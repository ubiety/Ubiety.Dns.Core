using System;
using System.Net;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Test.Records
{
    public class RecordATests
    {
        private readonly Byte[] data = new Byte[4];
        private readonly RecordReader reader;

        public RecordATests()
        {
            data[0] = 192;
            data[1] = 168;
            data[2] = 0;
            data[3] = 1;

            reader = new RecordReader(data);
        }

        public void TestRecordAddress()
        {
            var a = new RecordA(reader);

            a.Address.ShouldBe(IPAddress.Parse("192.168.0.1"));
        }

        public void TestRecordToString()
        {
            var a = new RecordA(reader);

            a.ToString().ShouldBe("192.168.0.1");
        }
    }
}