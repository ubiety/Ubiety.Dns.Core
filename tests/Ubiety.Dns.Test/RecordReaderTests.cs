/*
 * Copyright 2026 Dieter Lunn
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

using Shouldly;
using Ubiety.Dns.Core;
using Xunit;

namespace Ubiety.Dns.Test
{
    public class RecordReaderTests
    {
        // "example.com." occupying offsets 0..12, followed by "www" plus a pointer back to it.
        private static readonly byte[] CompressedName =
        [
            7, (byte)'e', (byte)'x', (byte)'a', (byte)'m', (byte)'p', (byte)'l', (byte)'e',
            3, (byte)'c', (byte)'o', (byte)'m',
            0,
            3, (byte)'w', (byte)'w', (byte)'w',
            0xC0, 0x00,
        ];

        [Fact]
        public void ReadDomainNameReadsUncompressedName()
        {
            var reader = new RecordReader(CompressedName);

            reader.ReadDomainName().ShouldBe("example.com.");
            reader.Position.ShouldBe(13);
        }

        [Fact]
        public void ReadDomainNameFollowsCompressionPointer()
        {
            var reader = new RecordReader(CompressedName, 13);

            reader.ReadDomainName().ShouldBe("www.example.com.");
        }

        [Fact]
        public void ReadDomainNameConsumesOnlyThePointerItself()
        {
            var reader = new RecordReader(CompressedName, 13);

            reader.ReadDomainName();

            // 3 label-length + 3 label bytes + 2 pointer bytes, and nothing from the target.
            reader.Position.ShouldBe(19);
        }

        [Fact]
        public void ReadDomainNameTerminatesOnSelfReferencingPointer()
        {
            var reader = new RecordReader([0xC0, 0x00]);

            reader.ReadDomainName().ShouldBe(".");
        }

        [Fact]
        public void ReadDomainNameTerminatesOnPointerCycle()
        {
            var reader = new RecordReader([0xC0, 0x02, 0xC0, 0x00]);

            reader.ReadDomainName().ShouldBe(".");
        }

        [Fact]
        public void ReadDomainNameTerminatesOnLabelPointerCycle()
        {
            // Each hop emits a label before jumping, so the name grows until the length bound stops it.
            byte[] data = [1, (byte)'a', 0xC0, 0x04, 1, (byte)'b', 0xC0, 0x00];

            var reader = new RecordReader(data);

            reader.ReadDomainName().Length.ShouldBeLessThanOrEqualTo(255);
        }

        [Fact]
        public void ReadDomainNameStopsOnOutOfRangePointer()
        {
            var reader = new RecordReader([0xC0, 0xFF]);

            reader.ReadDomainName().ShouldBe(".");
        }

        [Fact]
        public void ReadDomainNameStopsOnTruncatedName()
        {
            // Declares a 7 byte label but supplies 3, with no terminating zero.
            var reader = new RecordReader([7, (byte)'a', (byte)'b', (byte)'c']);

            reader.ReadDomainName().ShouldBe("abc.");
        }

        [Fact]
        public void ReadDomainNameReturnsRootForEmptyName()
        {
            var reader = new RecordReader([0]);

            reader.ReadDomainName().ShouldBe(".");
        }
    }
}
