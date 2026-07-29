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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records.General;
using Xunit;
using Builder = Ubiety.Dns.Test.DnsMessageBuilder;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Covers the UDP query path by substituting the transport, so the retry and failover logic is
    /// exercised without a socket.
    /// </summary>
    public class UdpRequestTests
    {
        private static readonly IPEndPoint First = new(IPAddress.Parse("192.0.2.1"), 53);
        private static readonly IPEndPoint Second = new(IPAddress.Parse("192.0.2.2"), 53);

        private static byte[] Reply(string name, params byte[] address) =>
            Builder.Message(
                0x1234, Builder.NoErrorFlags, name, QuestionType.A,
                Builder.ARecord(name, 300, address));

        private static Resolver UdpResolver(IUdpTransport transport, int retries = 1, params IPEndPoint[] servers) =>
            new(servers.Length > 0 ? servers : [First])
            {
                UdpTransport = transport,
                Timeout = 1000,
                Retries = retries,
                TransportType = TransportType.Udp,
            };

        [Fact]
        public void QueryReturnsTheParsedReply()
        {
            var transport = new FakeUdpTransport(_ => Reply("example.com", 5, 6, 7, 8));

            var response = UdpResolver(transport).Query("example.com", QuestionType.A);

            response.TimedOut.ShouldBeFalse();
            response.Server.ShouldBe(First);
            response.GetRecords<RecordA>().ShouldHaveSingleItem()
                .Address.ShouldBe(IPAddress.Parse("5.6.7.8"));
        }

        [Fact]
        public void QuerySendsTheEncodedQuestionAndTheConfiguredTimeout()
        {
            var transport = new FakeUdpTransport(_ => Reply("example.com", 1, 2, 3, 4));

            UdpResolver(transport).Query("example.com", QuestionType.A);

            var call = transport.Calls.ShouldHaveSingleItem();
            call.Server.ShouldBe(First);
            call.Timeout.ShouldBe(1000);

            // The bytes on the wire are a real query for the name that was asked for.
            var reader = new RecordReader(call.Request) { Position = 12 };
            reader.ReadDomainName().ShouldBe("example.com.");
            reader.ReadUInt16().ShouldBe((ushort)QuestionType.A);
        }

        [Fact]
        public void QueryFailsOverToTheNextServer()
        {
            var transport = new FakeUdpTransport(call =>
                call.Server.Equals(First) ? throw new SocketException() : Reply("example.com", 9, 9, 9, 9));

            var response = UdpResolver(transport, 1, First, Second).Query("example.com", QuestionType.A);

            response.TimedOut.ShouldBeFalse();
            response.Server.ShouldBe(Second);
            transport.Calls.Count.ShouldBe(2);
        }

        [Fact]
        public void QueryRetriesEveryServerBeforeGivingUp()
        {
            var transport = new FakeUdpTransport(_ => throw new SocketException());

            var response = UdpResolver(transport, 3, First, Second).Query("example.com", QuestionType.A);

            response.TimedOut.ShouldBeTrue();
            response.Answers.ShouldBeEmpty();

            // Three attempts across two servers.
            transport.Calls.Count.ShouldBe(6);
        }

        [Fact]
        public void QueryReturnsATimedOutResponseWhenEveryAttemptFails()
        {
            var transport = new FakeUdpTransport(_ => throw new SocketException());

            UdpResolver(transport).Query("example.com", QuestionType.A).TimedOut.ShouldBeTrue();
        }

        [Fact]
        public void ASuccessfulUdpQueryIsCached()
        {
            var transport = new FakeUdpTransport(_ => Reply("example.com", 5, 6, 7, 8));
            var resolver = new Resolver([First])
            {
                UdpTransport = transport,
                Timeout = 1000,
                Retries = 1,
                UseCache = true,
                TransportType = TransportType.Udp,
            };

            resolver.Query("example.com", QuestionType.A);
            resolver.Query("example.com", QuestionType.A);

            // The second query was served from the cache, so the transport saw only one call.
            transport.Calls.ShouldHaveSingleItem();
        }

        [Fact]
        public void AnExceptionThatIsNotASocketFailurePropagates()
        {
            // Only SocketException means "try the next server"; anything else is a real fault and
            // must not be swallowed by the retry loop.
            var transport = new FakeUdpTransport(_ => throw new InvalidOperationException("boom"));

            Should.Throw<InvalidOperationException>(
                () => UdpResolver(transport).Query("example.com", QuestionType.A));
        }

        private sealed record TransportCall(byte[] Request, IPEndPoint Server, int Timeout);

        private sealed class FakeUdpTransport(Func<TransportCall, byte[]> behaviour) : IUdpTransport
        {
            public List<TransportCall> Calls { get; } = [];

            public byte[] Exchange(byte[] request, IPEndPoint server, int timeout)
            {
                var call = new TransportCall(request, server, timeout);
                Calls.Add(call);
                return behaviour(call);
            }

            public Task<byte[]> ExchangeAsync(
                byte[] request, IPEndPoint server, int timeout, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(Exchange(request, server, timeout));
            }
        }
    }
}
