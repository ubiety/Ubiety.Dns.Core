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
using System.Linq;
using System.Net;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records.General;
using Xunit;

namespace Ubiety.Dns.Test
{
    public class ResponseTests
    {
        // A reply to "example.com A" carrying one answer with a 300 second TTL. The answer name is
        // the compression pointer 0xC00C back to the question, exactly as a real server sends it.
        private static readonly byte[] Reply =
        [
            0x12, 0x34, 0x81, 0x80, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
            7, (byte)'e', (byte)'x', (byte)'a', (byte)'m', (byte)'p', (byte)'l', (byte)'e',
            3, (byte)'c', (byte)'o', (byte)'m',
            0,
            0x00, 0x01, 0x00, 0x01,
            0xC0, 0x0C,
            0x00, 0x01, 0x00, 0x01,
            0x00, 0x00, 0x01, 0x2C,
            0x00, 0x04,
            5, 6, 7, 8,
        ];

        private static Response Parse() => new(new IPEndPoint(IPAddress.Loopback, 53), Reply);

        [Fact]
        public void ParsesHeaderQuestionAndAnswer()
        {
            var response = Parse();

            response.Header.ResponseCode.ShouldBe(ResponseCode.NoError);
            response.Questions.Count.ShouldBe(1);
            response.Questions[0].DomainName.ShouldBe("example.com.");
            response.Answers.Count.ShouldBe(1);
        }

        [Fact]
        public void ResolvesTheCompressedAnswerName()
        {
            var answer = Parse().Answers.Single();

            answer.Name.ShouldBe("example.com.");
            answer.Type.ShouldBe(RecordType.A);
            answer.TimeToLive.ShouldBe(300u);
        }

        [Fact]
        public void ExposesTheTypedRecord()
        {
            var response = Parse();

            var record = response.GetRecords<RecordA>().ShouldHaveSingleItem();
            record.Address.ShouldBe(IPAddress.Parse("5.6.7.8"));
        }

        [Fact]
        public void TimeStampIsUtc()
        {
            // IsExpired compares against DateTime.UtcNow, so a local timestamp would misjudge
            // expiry by the UTC offset and shift by an hour across a daylight saving transition.
            Parse().TimeStamp.Kind.ShouldBe(DateTimeKind.Utc);
        }

        [Fact]
        public void FreshRecordsAreNotExpired()
        {
            var response = Parse();

            response.ResourceRecords.ShouldAllBe(r => !r.IsExpired(response.TimeStamp));
        }

        [Fact]
        public void RecordsAreExpiredOnceTheTimeToLiveHasElapsed()
        {
            var answer = Parse().Answers.Single();

            answer.IsExpired(DateTime.UtcNow.AddSeconds(-400)).ShouldBeTrue();
            answer.IsExpired(DateTime.UtcNow.AddSeconds(-100)).ShouldBeFalse();
        }
    }
}
