/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Xunit;
using Builder = Ubiety.Dns.Test.DnsMessageBuilder;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Covers the TCP query path by substituting the transport, so connecting, framing, retries and
    /// failover are exercised without a socket.
    /// </summary>
    public class TcpRequestTests
    {
        private static readonly IPEndPoint First = new(IPAddress.Parse("192.0.2.1"), 53);
        private static readonly IPEndPoint Second = new(IPAddress.Parse("192.0.2.2"), 53);

        private static byte[] FramedReply(string name, params byte[] address) =>
            Builder.Framed(Builder.Message(
                0x1234, Builder.NoErrorFlags, name, QuestionType.A,
                Builder.ARecord(name, 300, address)));

        private static Resolver TcpResolver(ITcpTransport transport, int retries = 1, params IPEndPoint[] servers) =>
            new(servers.Length > 0 ? servers : [First])
            {
                TcpTransport = transport,
                Timeout = 1000,
                Retries = retries,
            };

        [Fact]
        public void TcpIsTheDefaultTransport()
        {
            new Resolver([First]).TransportType.ShouldBe(TransportType.Tcp);
        }

        [Fact]
        public void QueryReturnsTheParsedReply()
        {
            var transport = new FakeTcpTransport(_ => FramedReply("example.com", 5, 6, 7, 8));

            var response = TcpResolver(transport).Query("example.com", QuestionType.A);

            response.TimedOut.ShouldBeFalse();
            response.Server.ShouldBe(First);
            response.GetRecords<RecordA>().ShouldHaveSingleItem()
                .Address.ShouldBe(IPAddress.Parse("5.6.7.8"));
        }

        [Fact]
        public void QueryWritesALengthPrefixedQuestionAndPassesTheTimeout()
        {
            var transport = new FakeTcpTransport(_ => FramedReply("example.com", 1, 2, 3, 4));

            TcpResolver(transport).Query("example.com", QuestionType.A);

            var connection = transport.Connections.ShouldHaveSingleItem();
            connection.Server.ShouldBe(First);
            connection.Timeout.ShouldBe(1000);

            // Two byte big-endian length prefix, then exactly that many bytes of query.
            var written = connection.Written;
            var declared = (written[0] << 8) | written[1];
            declared.ShouldBe(written.Length - 2);

            var reader = new RecordReader(written[2..]) { Position = 12 };
            reader.ReadDomainName().ShouldBe("example.com.");
            reader.ReadUInt16().ShouldBe((ushort)QuestionType.A);
        }

        [Fact]
        public void QueryFailsOverToTheNextServer()
        {
            var transport = new FakeTcpTransport(
                server => server.Equals(First) ? throw new SocketException() : FramedReply("example.com", 9, 9, 9, 9));

            var response = TcpResolver(transport, 1, First, Second).Query("example.com", QuestionType.A);

            response.Server.ShouldBe(Second);
            transport.Attempts.Count.ShouldBe(2);
        }

        [Fact]
        public void QueryRetriesEveryServerBeforeGivingUp()
        {
            var transport = new FakeTcpTransport(_ => throw new SocketException());

            var response = TcpResolver(transport, 3, First, Second).Query("example.com", QuestionType.A);

            response.TimedOut.ShouldBeTrue();
            transport.Attempts.Count.ShouldBe(6);
        }

        [Fact]
        public void AConnectionThatFailsMidStreamIsTreatedAsAFailedServer()
        {
            // A truncated reply raises EndOfStreamException, which derives from IOException and so
            // means "try the next server" rather than aborting the run.
            var truncated = FramedReply("example.com", 1, 2, 3, 4)[..6];
            var transport = new FakeTcpTransport(
                server => server.Equals(First) ? truncated : FramedReply("example.com", 7, 7, 7, 7));

            var response = TcpResolver(transport, 1, First, Second).Query("example.com", QuestionType.A);

            response.Server.ShouldBe(Second);
            response.GetRecords<RecordA>().ShouldHaveSingleItem()
                .Address.ShouldBe(IPAddress.Parse("7.7.7.7"));
        }

        [Fact]
        public void TheConnectionIsDisposedAfterAQuery()
        {
            var transport = new FakeTcpTransport(_ => FramedReply("example.com", 5, 6, 7, 8));

            TcpResolver(transport).Query("example.com", QuestionType.A);

            transport.Connections.ShouldHaveSingleItem().Disposed.ShouldBeTrue();
        }

        [Fact]
        public void AZoneTransferReadsEveryMessageFromTheSameConnection()
        {
            var transfer = Builder.Framed(
                Builder.Message(1, Builder.NoErrorFlags, "example.com", QuestionType.AXFR,
                    Builder.SoaRecord("example.com", 1)),
                Builder.Message(1, Builder.NoErrorFlags, "example.com", QuestionType.AXFR,
                    Builder.ARecord("www.example.com", 300, 10, 0, 0, 1)),
                Builder.Message(1, Builder.NoErrorFlags, "example.com", QuestionType.AXFR,
                    Builder.SoaRecord("example.com", 1)));

            var transport = new FakeTcpTransport(_ => transfer);

            var response = TcpResolver(transport).Query("example.com", QuestionType.AXFR);

            response.Answers.Count.ShouldBe(3);
            response.GetRecords<RecordSoa>().Count.ShouldBe(2);
            transport.Connections.ShouldHaveSingleItem();
        }

        [Fact]
        public void AnExceptionThatIsNotASocketOrIoFailurePropagates()
        {
            var transport = new FakeTcpTransport(_ => throw new InvalidOperationException("boom"));

            Should.Throw<InvalidOperationException>(
                () => TcpResolver(transport).Query("example.com", QuestionType.A));
        }

        private sealed class FakeTcpTransport(Func<IPEndPoint, byte[]> behaviour) : ITcpTransport
        {
            public List<RecordedConnection> Connections { get; } = [];

            public List<IPEndPoint> Attempts { get; } = [];

            public ITcpConnection Connect(IPEndPoint server, int timeout)
            {
                Attempts.Add(server);

                // behaviour throws for a server that should fail to connect.
                var reply = behaviour(server);

                var connection = new RecordedConnection(reply, server, timeout);
                Connections.Add(connection);
                return connection;
            }

            public Task<ITcpConnection> ConnectAsync(
                IPEndPoint server, int timeout, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(Connect(server, timeout));
            }
        }

        /// <summary>
        /// A connection whose stream accepts the query, then serves the canned reply.
        /// </summary>
        private sealed class RecordedConnection : ITcpConnection
        {
            private readonly DuplexStream _stream;

            internal RecordedConnection(byte[] reply, IPEndPoint server, int timeout)
            {
                _stream = new DuplexStream(reply);
                Server = server;
                Timeout = timeout;
            }

            public IPEndPoint Server { get; }

            public int Timeout { get; }

            public bool Disposed { get; private set; }

            public byte[] Written => _stream.Written;

            public Stream Stream => _stream;

            public void Dispose() => Disposed = true;
        }

        /// <summary>
        /// Writes are captured, reads come from a fixed reply, so one stream stands in for a socket.
        /// </summary>
        private sealed class DuplexStream(byte[] reply) : Stream
        {
            private readonly MemoryStream _writes = new();
            private readonly MemoryStream _reads = new(reply);

            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => _reads.Length;

            public override long Position
            {
                get => _reads.Position;
                set => _reads.Position = value;
            }

            internal byte[] Written => _writes.ToArray();

            public override void Flush() => _writes.Flush();

            public override int Read(byte[] buffer, int offset, int count) => _reads.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

            public override void SetLength(long value) => throw new NotSupportedException();

            public override void Write(byte[] buffer, int offset, int count) => _writes.Write(buffer, offset, count);

            public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
                _reads.ReadAsync(buffer, cancellationToken);

            public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) =>
                _writes.WriteAsync(buffer, cancellationToken);

            public override Task FlushAsync(CancellationToken cancellationToken) => _writes.FlushAsync(cancellationToken);
        }
    }
}
