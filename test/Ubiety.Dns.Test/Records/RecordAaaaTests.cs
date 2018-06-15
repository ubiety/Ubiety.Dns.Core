using System;
using System.Net;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records;
using Xunit;

namespace Ubiety.Dns.Test.Records
{
    public class RecordAaaaTests
    {
        private readonly Byte[] data = new Byte[16];
        private readonly RecordReader reader;

        public RecordAaaaTests()
        {
            data[0] = 0x20;
            data[1] = 0x01;
            data[2] = 0x0D;
            data[3] = 0xB8;
            data[4] = 0x85;
            data[5] = 0xA3;
            data[6] = 0x00;
            data[7] = 0x00;
            data[8] = 0x00;
            data[9] = 0x00;
            data[10] = 0x8A;
            data[11] = 0x2E;
            data[12] = 0x03;
            data[13] = 0x70;
            data[14] = 0x73;
            data[15] = 0x34;

            reader = new RecordReader(data);
        }

        [Fact]
        public void TestRecordAddress()
        {
            var a = new RecordAaaa(reader);

            a.Address.ShouldBe(IPAddress.Parse("2001:0db8:85a3:0000:0000:8a2e:0370:7334"));
        }

        [Fact]
        public void TestRecordToString()
        {
            var a = new RecordAaaa(reader);

            a.ToString().ShouldBe("2001:db8:85a3::8a2e:370:7334");
        }
    }
}