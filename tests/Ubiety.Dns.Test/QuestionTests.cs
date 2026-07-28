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
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Xunit;

namespace Ubiety.Dns.Test
{
    public class QuestionTests
    {
        [Fact]
        public void GetBytesEncodesNameAsLengthPrefixedLabels()
        {
            var question = new Question("example.com", QuestionType.A, QuestionClass.IN);

            var bytes = question.GetBytes().ToArray();

            // 07 "example" 03 "com" 00, then QTYPE A and QCLASS IN. No literal separators.
            bytes.ShouldBe(
            [
                7, (byte)'e', (byte)'x', (byte)'a', (byte)'m', (byte)'p', (byte)'l', (byte)'e',
                3, (byte)'c', (byte)'o', (byte)'m',
                0,
                0, 1,
                0, 1,
            ]);
        }

        [Fact]
        public void GetBytesDoesNotEmitASeparatorByte()
        {
            var question = new Question("example.com", QuestionType.A, QuestionClass.IN);

            var name = question.GetBytes().ToArray()[..^4];

            // A stray 0x2E is read by the server as a 46 octet label and the query is dropped.
            name.ShouldNotContain((byte)'.');
            name.Length.ShouldBe(13);
        }

        [Theory]
        [InlineData("example.com")]
        [InlineData("www.microsoft.com")]
        [InlineData("a.b.c.d.e.f.example.org")]
        [InlineData("xn--bcher-kva.example")]
        [InlineData("single")]
        public void GetBytesRoundTripsThroughRecordReader(string domainName)
        {
            var question = new Question(domainName, QuestionType.MX, QuestionClass.IN);

            var reader = new RecordReader(question.GetBytes().ToArray());

            reader.ReadDomainName().ShouldBe($"{domainName}.");
            reader.ReadUInt16().ShouldBe((ushort)QuestionType.MX);
            reader.ReadUInt16().ShouldBe((ushort)QuestionClass.IN);
        }

        [Fact]
        public void GetBytesRoundTripsTheRootName()
        {
            var question = new Question(".", QuestionType.NS, QuestionClass.IN);

            var bytes = question.GetBytes().ToArray();
            var reader = new RecordReader(bytes);

            bytes.Length.ShouldBe(5);
            reader.ReadDomainName().ShouldBe(".");
            reader.ReadUInt16().ShouldBe((ushort)QuestionType.NS);
        }

        [Fact]
        public void GetBytesTreatsTrailingSeparatorAsOptional()
        {
            var withDot = new Question("example.com.", QuestionType.A, QuestionClass.IN);
            var withoutDot = new Question("example.com", QuestionType.A, QuestionClass.IN);

            withoutDot.GetBytes().ShouldBe(withDot.GetBytes());
        }

        [Fact]
        public void GetBytesRoundTripsAMaximumLengthLabel()
        {
            var label = new string('a', 63);
            var question = new Question($"{label}.com", QuestionType.A, QuestionClass.IN);

            var reader = new RecordReader(question.GetBytes().ToArray());

            reader.ReadDomainName().ShouldBe($"{label}.com.");
        }

        [Fact]
        public void GetBytesRejectsAnOversizedLabel()
        {
            var question = new Question($"{new string('a', 64)}.com", QuestionType.A, QuestionClass.IN);

            Should.Throw<FormatException>(() => question.GetBytes().ToArray());
        }

        [Fact]
        public void GetBytesRejectsAnEmptyLabel()
        {
            var question = new Question("example..com", QuestionType.A, QuestionClass.IN);

            Should.Throw<FormatException>(() => question.GetBytes().ToArray());
        }

        [Fact]
        public void GetBytesRejectsAnOversizedName()
        {
            // Four 63 octet labels encode to 260 octets once length bytes and the root are added.
            var label = new string('a', 63);
            var question = new Question(
                string.Join('.', Enumerable.Repeat(label, 4)), QuestionType.A, QuestionClass.IN);

            Should.Throw<FormatException>(() => question.GetBytes().ToArray());
        }
    }
}
