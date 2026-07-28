/*
 * Copyright 2020 Dieter Lunn
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
