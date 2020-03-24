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

using System.Net;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records.General;
using Xunit;

namespace Ubiety.Dns.Test.Records
{
    public class RecordATests
    {
        public RecordATests()
        {
            _data[0] = 192;
            _data[1] = 168;
            _data[2] = 0;
            _data[3] = 1;

            _reader = new RecordReader(_data);
        }

        private readonly byte[] _data = new byte[4];
        private readonly RecordReader _reader;

        [Fact]
        public void TestRecordAddress()
        {
            var a = new RecordA(_reader);

            a.Address.ShouldBe(IPAddress.Parse("192.168.0.1"));
        }

        [Fact]
        public void TestRecordToString()
        {
            var a = new RecordA(_reader);

            a.ToString().ShouldBe("192.168.0.1");
        }
    }
}