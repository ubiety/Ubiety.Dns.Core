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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records;
using Xunit;

namespace Ubiety.Dns.Test.Records
{
    public class RecordSrvTests
    {
        [Fact]
        public void TestSrvRecordCreate()
        {
            var record = new RecordSrv(GetReader(10, 20, 80));
            record.Priority.ShouldBe((ushort)10);
            record.Weight.ShouldBe((ushort)20);
            record.Port.ShouldBe((ushort)80);
            record.Target.ShouldBe("test.com.");
        }

        [Fact]
        public void TestSrvGreaterThanWithHigherWeight()
        {
            var left = new RecordSrv(GetReader(10, 30, 80));
            var right = new RecordSrv(GetReader(10, 20, 80));

            left.ShouldBeGreaterThan(right);
        }

        [Fact]
        public void TestSrvGreaterThanWithHigherPriority()
        {
            var left = new RecordSrv(GetReader(20, 10, 80));
            var right = new RecordSrv(GetReader(10, 10, 80));

            left.ShouldBeGreaterThan(right);
        }

        [Fact]
        public void TestSrvRecordsAreEqual()
        {
            var left = new RecordSrv(GetReader(10,10,80));
            var right = new RecordSrv(GetReader(10,10,80));

            left.Priority.ShouldBe(right.Priority);
            left.Weight.ShouldBe(right.Weight);
            left.Port.ShouldBe(right.Port);
            left.Target.ShouldBe(right.Target);
        }

        [Fact]
        public void TestSrvToString()
        {
            var record = new RecordSrv(GetReader(10,10,80));

            record.ToString().ShouldBe("10 10 80 test.com.");
        }

        [Fact]
        public void TestSrvNotEqualToNull()
        {
            var left = new RecordSrv(GetReader(10, 10 ,80));

            left.ShouldNotBeNull();
        }

        private static RecordReader GetReader(ushort priority, ushort weight, ushort port, string target = "test.com")
        {
            var data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(priority).Reverse());
            data.AddRange(BitConverter.GetBytes(weight).Reverse());
            data.AddRange(BitConverter.GetBytes(port).Reverse());
            var domain = Encoding.UTF8.GetBytes(target);
            data.Add((byte)domain.Length);
            var record = data.Concat(domain).ToArray();
            return new RecordReader(record);
        }
    }
}
