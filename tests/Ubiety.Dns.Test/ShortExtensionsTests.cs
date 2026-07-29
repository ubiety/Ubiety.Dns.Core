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

using System.Linq;
using Shouldly;
using Ubiety.Dns.Core.Common.Extensions;
using Xunit;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Covers the bit helpers the DNS header flags are built from.
    /// </summary>
    public class ShortExtensionsTests
    {
        [Fact]
        public void GetBytesWritesNetworkByteOrder()
        {
            ((ushort)0x1234).GetBytes().ToArray().ShouldBe([0x12, 0x34]);
            ((ushort)0x00FF).GetBytes().ToArray().ShouldBe([0x00, 0xFF]);
            ushort.MaxValue.GetBytes().ToArray().ShouldBe([0xFF, 0xFF]);
        }

        [Theory]
        [InlineData(0, 0, true, 1)]
        [InlineData(0, 7, true, 128)]
        [InlineData(0, 15, true, 32768)]
        [InlineData(0xFFFF, 0, false, 0xFFFE)]
        [InlineData(0xFFFF, 15, false, 0x7FFF)]
        public void SetFlagSetsASingleBit(ushort value, int position, bool flag, ushort expected)
        {
            value.SetFlag(position, flag).ShouldBe(expected);
        }

        [Theory]
        [InlineData(0, 0, 4, 0xF, 0x000F)]
        [InlineData(0, 4, 4, 0xF, 0x00F0)]
        [InlineData(0xFFFF, 4, 4, 0x0, 0xFF0F)]
        public void SetFlagSetsAMultiBitField(ushort value, int position, int length, ushort flag, ushort expected)
        {
            value.SetFlag(position, length, flag).ShouldBe(expected);
        }

        [Fact]
        public void SetFlagMasksAValueWiderThanItsField()
        {
            // 0xFF does not fit in four bits and is truncated rather than bleeding into neighbours.
            ((ushort)0).SetFlag(0, 4, 0xFF).ShouldBe((ushort)0x000F);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 16)]
        [InlineData(0, 20)]
        public void SetFlagIsANoOpForAnInvalidLengthOrPosition(int length, int position)
        {
            ((ushort)0x1234).SetFlag(position, length, 0xF).ShouldBe((ushort)0x1234);
        }

        [Theory]
        [InlineData(0x0001, 0, true)]
        [InlineData(0x0001, 1, false)]
        [InlineData(0x8000, 15, true)]
        [InlineData(0x0000, 15, false)]
        public void GetFlagReadsASingleBit(ushort value, int position, bool expected)
        {
            value.GetFlag(position).ShouldBe(expected);
        }

        [Theory]
        [InlineData(0x00F0, 4, 4, 0xF)]
        [InlineData(0x000F, 0, 4, 0xF)]
        [InlineData(0x1234, 0, 4, 0x4)]
        [InlineData(0x1234, 12, 4, 0x1)]
        public void GetFlagReadsAMultiBitField(ushort value, int position, int length, ushort expected)
        {
            value.GetFlag(position, length).ShouldBe(expected);
        }

        [Theory]
        [InlineData(0x1234, 0, 4, 0xF)]
        [InlineData(0x0000, 8, 8, 0xAB)]
        public void SetFlagAndGetFlagRoundTrip(ushort value, int position, int length, ushort flag)
        {
            value.SetFlag(position, length, flag).GetFlag(position, length).ShouldBe(flag);
        }
    }
}
