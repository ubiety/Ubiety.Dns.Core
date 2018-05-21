using System;
using System.Net;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records;
using Xunit;

namespace Ubiety.Dns.Test.Records
{
    public class RecordATest
    {
        [Fact]
        public void TestAddress()
        {
            Byte[] data = new Byte[4];
            data[0] = 192;
            data[1] = 168;
            data[2] = 0;
            data[3] = 1;

            var reader = new RecordReader(data);

            var a = new RecordA(reader);

            Assert.Equal(a.Address, IPAddress.Parse("192.168.0.1"));
        }
    }
}