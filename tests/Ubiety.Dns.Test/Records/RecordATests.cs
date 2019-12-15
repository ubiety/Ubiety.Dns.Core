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