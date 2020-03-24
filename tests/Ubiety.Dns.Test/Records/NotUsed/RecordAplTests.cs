/* 
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *  
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *  
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *  
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records.NotUsed;
using Xunit;

namespace Ubiety.Dns.Test.Records.NotUsed
{
    public class RecordAplTests
    {
        public RecordAplTests()
        {
            _data = new byte[4];
            _data[0] = 0;
            _data[1] = 2;
            _data[2] = 4;
            _data[3] = 5;

            _reader = new RecordReader(_data, 2);
        }

        private readonly byte[] _data;
        private readonly RecordReader _reader;

        [Fact]
        public void TestRecordData()
        {
            var apl = new RecordApl(_reader);

            apl.RecordData[0].ShouldBe(_data[2]);
        }

        [Fact]
        public void TestToString()
        {
            var apl = new RecordApl(_reader);

            apl.ToString().ShouldMatch("RecordApl is not-used");
        }
    }
}