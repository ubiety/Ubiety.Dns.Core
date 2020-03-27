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
            
            left.ShouldBe(right);
        }

        [Fact]
        public void TestSrvToString()
        {
            var record = new RecordSrv(GetReader(10,10,80));
            
            record.ToString().ShouldBe("10 10 80 test.com.");
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