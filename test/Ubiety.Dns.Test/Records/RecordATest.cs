using System;
using System.Net;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records;
using Xunit;

namespace Ubiety.Dns.Test.Records
{
    public class RecordATest
    {
        private readonly Byte[] data = new Byte[4];
        private readonly RecordReader reader;

        public RecordATest()
        {
            data[0] = 192;
            data[1] = 168;
            data[2] = 0;
            data[3] = 1;

            reader = new RecordReader(data);
        }

        [Fact]
        public void TestRecordAddress()
        {
            var a = new RecordA(reader);

            Assert.Equal(IPAddress.Parse("192.168.0.1"), a.Address);
        }

        [Fact]
        public void TestRecordToString()
        {
            var a = new RecordA(reader);

            Assert.Equal("192.168.0.1", a.ToString());
        }
    }
}