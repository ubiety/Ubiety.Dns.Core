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

using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Xunit;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Covers <see cref="Resolver.ReceiveResponse"/>, which frames and parses DNS over TCP. It reads
    /// from a stream rather than a socket, so a MemoryStream exercises it without any network.
    /// </summary>
    public class ReceiveResponseTests
    {
        private static readonly IPEndPoint Server = new(IPAddress.Loopback, 53);

        private static Resolver BuildResolver() =>
            ResolverBuilder.Begin().AddDnsServer(IPAddress.Loopback).Build();

        private static Response Receive(byte[] framed) =>
            BuildResolver().ReceiveResponse(new MemoryStream(framed), Server);

        [Fact]
        public void ReadsASingleFramedResponse()
        {
            var message = DnsMessageBuilder.Message(
                0x1234,
                DnsMessageBuilder.NoErrorFlags,
                "example.com",
                QuestionType.A,
                DnsMessageBuilder.ARecord("example.com", 300, 5, 6, 7, 8));

            var response = Receive(DnsMessageBuilder.Framed(message));

            response.Header.ResponseCode.ShouldBe(ResponseCode.NoError);
            response.Questions.ShouldHaveSingleItem().DomainName.ShouldBe("example.com.");
            response.MessageSize.ShouldBe(message.Length);
            response.GetRecords<RecordA>().ShouldHaveSingleItem()
                .Address.ShouldBe(IPAddress.Parse("5.6.7.8"));
        }

        [Fact]
        public void ReturnsTheServerAsTheResponseSource()
        {
            var message = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.A,
                DnsMessageBuilder.ARecord("example.com", 60, 1, 2, 3, 4));

            Receive(DnsMessageBuilder.Framed(message)).Server.ShouldBe(Server);
        }

        [Fact]
        public void ReturnsImmediatelyOnAnErrorResponseCode()
        {
            // A second message follows; hitting it would mean the error was not short-circuited.
            var failure = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.ServFailFlags, "example.com", QuestionType.A);
            var unexpected = DnsMessageBuilder.Message(
                2, DnsMessageBuilder.NoErrorFlags, "other.example", QuestionType.A,
                DnsMessageBuilder.ARecord("other.example", 60, 9, 9, 9, 9));

            var response = Receive(DnsMessageBuilder.Framed(failure, unexpected));

            response.Header.ResponseCode.ShouldBe(ResponseCode.ServFail);
            response.Answers.ShouldBeEmpty();
        }

        [Fact]
        public void StopsAfterTheFirstMessageForANonTransferQuery()
        {
            var first = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.A,
                DnsMessageBuilder.ARecord("example.com", 60, 1, 1, 1, 1));
            var second = DnsMessageBuilder.Message(
                2, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.A,
                DnsMessageBuilder.ARecord("example.com", 60, 2, 2, 2, 2));

            var response = Receive(DnsMessageBuilder.Framed(first, second));

            response.Answers.ShouldHaveSingleItem();
            response.MessageSize.ShouldBe(first.Length);
        }

        [Fact]
        public void AccumulatesAZoneTransferUntilTheClosingSoa()
        {
            // A real AXFR opens and closes with an SOA; everything between is the zone.
            var opening = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.AXFR,
                DnsMessageBuilder.SoaRecord("example.com", 2026072801));
            var middle = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.AXFR,
                DnsMessageBuilder.ARecord("www.example.com", 300, 10, 0, 0, 1),
                DnsMessageBuilder.ARecord("mail.example.com", 300, 10, 0, 0, 2));
            var closing = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.AXFR,
                DnsMessageBuilder.SoaRecord("example.com", 2026072801));

            var response = Receive(DnsMessageBuilder.Framed(opening, middle, closing));

            response.Answers.Count.ShouldBe(4);
            response.Header.AnswerCount.ShouldBe((ushort)4);
            response.Header.QuestionCount.ShouldBe((ushort)1);
            response.MessageSize.ShouldBe(opening.Length + middle.Length + closing.Length);
            response.GetRecords<RecordA>().Count.ShouldBe(2);
            response.GetRecords<RecordSoa>().Count.ShouldBe(2);
        }

        [Fact]
        public void KeepsReadingAZoneTransferUntilTheSecondSoaArrives()
        {
            // Only one SOA so far, so the transfer is incomplete and the stream runs out.
            var opening = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.AXFR,
                DnsMessageBuilder.SoaRecord("example.com", 1));
            var middle = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.AXFR,
                DnsMessageBuilder.ARecord("www.example.com", 300, 10, 0, 0, 1));

            Should.Throw<SocketException>(
                () => Receive(DnsMessageBuilder.Framed(opening, middle)));
        }

        [Fact]
        public void ThrowsWhenTheStreamEndsBeforeALengthPrefix()
        {
            Should.Throw<SocketException>(() => Receive([]));
        }

        [Fact]
        public void ThrowsWhenTheLengthPrefixIsIncomplete()
        {
            Should.Throw<SocketException>(() => Receive([0x00]));
        }

        [Fact]
        public void ThrowsWhenTheLengthPrefixIsZero()
        {
            Should.Throw<SocketException>(() => Receive([0x00, 0x00]));
        }

        [Fact]
        public void ThrowsWhenTheMessageIsShorterThanItsLengthPrefix()
        {
            var message = DnsMessageBuilder.Message(
                1, DnsMessageBuilder.NoErrorFlags, "example.com", QuestionType.A,
                DnsMessageBuilder.ARecord("example.com", 60, 1, 2, 3, 4));

            // Claim the full length but supply only half the body. A plain Stream.Read would return
            // a short buffer and the remainder would be parsed as zeroed bytes.
            var truncated = DnsMessageBuilder.Framed(message).Take(2 + (message.Length / 2)).ToArray();

            Should.Throw<EndOfStreamException>(() => Receive(truncated));
        }
    }
}
