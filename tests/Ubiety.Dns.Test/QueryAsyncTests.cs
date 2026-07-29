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
    /// Covers QueryAsync over both transports. The async path is a separate implementation rather
    /// than the synchronous one bridged onto a task, so it needs its own coverage.
    /// </summary>
    public class QueryAsyncTests
    {
        private static readonly IPEndPoint First = new(IPAddress.Parse("192.0.2.1"), 53);
        private static readonly IPEndPoint Second = new(IPAddress.Parse("192.0.2.2"), 53);

        private static byte[] Reply(string name, params byte[] address) =>
            Builder.Message(
                0x1234, Builder.NoErrorFlags, name, QuestionType.A,
                Builder.ARecord(name, 300, address));

        [Fact]
        public async Task UdpQueryAsyncReturnsTheParsedReply()
        {
            var resolver = new Resolver([First])
            {
                UdpTransport = new StubUdp(_ => Reply("example.com", 5, 6, 7, 8)),
                Timeout = 1000,
                Retries = 1,
                TransportType = TransportType.Udp,
            };

            var response = await resolver.QueryAsync("example.com", QuestionType.A);

            response.TimedOut.ShouldBeFalse();
            response.GetRecords<RecordA>().ShouldHaveSingleItem()
                .Address.ShouldBe(IPAddress.Parse("5.6.7.8"));
        }

        [Fact]
        public async Task TcpQueryAsyncReturnsTheParsedReply()
        {
            var resolver = TcpResolver(_ => Builder.Framed(Reply("example.com", 1, 2, 3, 4)));

            var response = await resolver.QueryAsync("example.com", QuestionType.A);

            response.GetRecords<RecordA>().ShouldHaveSingleItem()
                .Address.ShouldBe(IPAddress.Parse("1.2.3.4"));
        }

        [Fact]
        public async Task TcpQueryAsyncWritesALengthPrefixedQuestion()
        {
            var transport = new StubTcp(_ => Builder.Framed(Reply("example.com", 1, 2, 3, 4)));
            var resolver = new Resolver([First]) { TcpTransport = transport, Timeout = 1000, Retries = 1 };

            await resolver.QueryAsync("example.com", QuestionType.A);

            var written = transport.Connections.ShouldHaveSingleItem().Written;
            (((written[0] << 8) | written[1])).ShouldBe(written.Length - 2);

            var reader = new RecordReader(written[2..]) { Position = 12 };
            reader.ReadDomainName().ShouldBe("example.com.");
        }

        [Fact]
        public async Task QueryAsyncFailsOverToTheNextServer()
        {
            var resolver = new Resolver([First, Second])
            {
                UdpTransport = new StubUdp(server =>
                    server.Equals(First) ? throw new SocketException() : Reply("example.com", 9, 9, 9, 9)),
                Timeout = 1000,
                Retries = 1,
                TransportType = TransportType.Udp,
            };

            (await resolver.QueryAsync("example.com", QuestionType.A)).Server.ShouldBe(Second);
        }

        [Fact]
        public async Task QueryAsyncReturnsATimedOutResponseWhenEveryAttemptFails()
        {
            var resolver = new Resolver([First])
            {
                UdpTransport = new StubUdp(_ => throw new SocketException()),
                Timeout = 1000,
                Retries = 2,
                TransportType = TransportType.Udp,
            };

            (await resolver.QueryAsync("example.com", QuestionType.A)).TimedOut.ShouldBeTrue();
        }

        [Fact]
        public async Task QueryAsyncServesFromTheCacheWithoutCallingTheTransport()
        {
            var transport = new StubUdp(_ => Reply("example.com", 5, 6, 7, 8));
            var resolver = new Resolver([First])
            {
                UdpTransport = transport,
                Timeout = 1000,
                Retries = 1,
                UseCache = true,
                TransportType = TransportType.Udp,
            };

            await resolver.QueryAsync("example.com", QuestionType.A);
            await resolver.QueryAsync("example.com", QuestionType.A);

            transport.Calls.ShouldBe(1);
        }

        [Fact]
        public async Task QueryAsyncThrowsWhenTheResolverHasNoServers()
        {
            await Should.ThrowAsync<InvalidOperationException>(
                () => new Resolver([]).QueryAsync("example.com", QuestionType.A));
        }

        [Fact]
        public async Task QueryAsyncObservesCancellation()
        {
            // Cancellation is not a timeout: it abandons the query rather than failing over, so it
            // must escape the retry loop instead of being swallowed like a SocketException.
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            var resolver = new Resolver([First, Second])
            {
                UdpTransport = new StubUdp(_ => Reply("example.com", 1, 1, 1, 1)),
                Timeout = 1000,
                Retries = 3,
                TransportType = TransportType.Udp,
            };

            await Should.ThrowAsync<OperationCanceledException>(
                () => resolver.QueryAsync("example.com", QuestionType.A, QuestionClass.IN, cts.Token));
        }

        [Fact]
        public async Task EveryOverloadShapeResolvesWithoutAmbiguity()
        {
            // This compiling is most of the test: a CancellationToken does not convert to a
            // QuestionClass, so the three argument overload cannot collide with the defaulted one.
            var resolver = UdpResolver(_ => Reply("example.com", 1, 2, 3, 4));
            using var cts = new CancellationTokenSource();

            var withDefaults = await resolver.QueryAsync("example.com", QuestionType.A);
            var withToken = await resolver.QueryAsync("example.com", QuestionType.A, cts.Token);
            var withClass = await resolver.QueryAsync("example.com", QuestionType.A, QuestionClass.IN);
            var withBoth = await resolver.QueryAsync(
                "example.com", QuestionType.A, QuestionClass.IN, cts.Token);

            foreach (var response in new[] { withDefaults, withToken, withClass, withBoth })
            {
                response.GetRecords<RecordA>().ShouldHaveSingleItem()
                    .Address.ShouldBe(IPAddress.Parse("1.2.3.4"));
            }
        }

        [Fact]
        public async Task TheTokenOverloadUsesTheInternetClass()
        {
            var resolver = UdpResolver(_ => Reply("example.com", 1, 2, 3, 4));

            var response = await resolver.QueryAsync(
                "example.com", QuestionType.A, CancellationToken.None);

            response.Questions.ShouldHaveSingleItem().QuestionClass.ShouldBe(QuestionClass.IN);
        }

        [Fact]
        public async Task TheTokenOverloadObservesCancellation()
        {
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            var resolver = UdpResolver(_ => Reply("example.com", 1, 2, 3, 4));

            await Should.ThrowAsync<OperationCanceledException>(
                () => resolver.QueryAsync("example.com", QuestionType.A, cts.Token));
        }

        [Fact]
        public async Task QueryAsyncAccumulatesAZoneTransfer()
        {
            var transfer = Builder.Framed(
                Builder.Message(1, Builder.NoErrorFlags, "example.com", QuestionType.AXFR,
                    Builder.SoaRecord("example.com", 1)),
                Builder.Message(1, Builder.NoErrorFlags, "example.com", QuestionType.AXFR,
                    Builder.ARecord("www.example.com", 300, 10, 0, 0, 1)),
                Builder.Message(1, Builder.NoErrorFlags, "example.com", QuestionType.AXFR,
                    Builder.SoaRecord("example.com", 1)));

            var response = await TcpResolver(_ => transfer).QueryAsync("example.com", QuestionType.AXFR);

            response.Answers.Count.ShouldBe(3);
            response.GetRecords<RecordSoa>().Count.ShouldBe(2);
        }

        [Fact]
        public async Task SyncAndAsyncAgreeOnTheSameReply()
        {
            var bytes = Reply("example.com", 4, 3, 2, 1);

            var sync = new Resolver([First])
            {
                UdpTransport = new StubUdp(_ => bytes), Timeout = 1000, Retries = 1,
                TransportType = TransportType.Udp,
            }.Query("example.com", QuestionType.A);

            var async = await new Resolver([First])
            {
                UdpTransport = new StubUdp(_ => bytes), Timeout = 1000, Retries = 1,
                TransportType = TransportType.Udp,
            }.QueryAsync("example.com", QuestionType.A);

            async.Header.ResponseCode.ShouldBe(sync.Header.ResponseCode);
            async.Answers.Count.ShouldBe(sync.Answers.Count);
            async.GetRecords<RecordA>()[0].Address.ShouldBe(sync.GetRecords<RecordA>()[0].Address);
        }

        private static Resolver UdpResolver(Func<IPEndPoint, byte[]> behaviour) =>
            new([First])
            {
                UdpTransport = new StubUdp(behaviour),
                Timeout = 1000,
                Retries = 1,
                TransportType = TransportType.Udp,
            };

        private static Resolver TcpResolver(Func<IPEndPoint, byte[]> behaviour) =>
            new([First]) { TcpTransport = new StubTcp(behaviour), Timeout = 1000, Retries = 1 };

        private sealed class StubUdp(Func<IPEndPoint, byte[]> behaviour) : IUdpTransport
        {
            public int Calls { get; private set; }

            public byte[] Exchange(byte[] request, IPEndPoint server, int timeout)
            {
                Calls++;
                return behaviour(server);
            }

            public Task<byte[]> ExchangeAsync(
                byte[] request, IPEndPoint server, int timeout, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(Exchange(request, server, timeout));
            }
        }

        private sealed class StubTcp(Func<IPEndPoint, byte[]> behaviour) : ITcpTransport
        {
            public List<StubConnection> Connections { get; } = [];

            public ITcpConnection Connect(IPEndPoint server, int timeout)
            {
                var connection = new StubConnection(behaviour(server));
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

        private sealed class StubConnection(byte[] reply) : ITcpConnection
        {
            private readonly MemoryStream _writes = new();
            private readonly MemoryStream _reads = new(reply);

            public byte[] Written => _writes.ToArray();

            public Stream Stream => new PairStream(_writes, _reads);

            public void Dispose()
            {
            }
        }

        /// <summary>Writes go one way, reads come from the canned reply.</summary>
        private sealed class PairStream(MemoryStream writes, MemoryStream reads) : Stream
        {
            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => reads.Length;

            public override long Position
            {
                get => reads.Position;
                set => reads.Position = value;
            }

            public override void Flush() => writes.Flush();

            public override Task FlushAsync(CancellationToken cancellationToken) => writes.FlushAsync(cancellationToken);

            public override int Read(byte[] buffer, int offset, int count) => reads.Read(buffer, offset, count);

            public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
                reads.ReadAsync(buffer, cancellationToken);

            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

            public override void SetLength(long value) => throw new NotSupportedException();

            public override void Write(byte[] buffer, int offset, int count) => writes.Write(buffer, offset, count);

            public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) =>
                writes.WriteAsync(buffer, cancellationToken);
        }
    }
}
