using System.Net;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records.General;
using Xunit;

namespace Ubiety.Dns.Test.Records
{
    public class RecordAaaaTests
    {
        public RecordAaaaTests()
        {
            _data[0] = 0x20;
            _data[1] = 0x01;
            _data[2] = 0x0D;
            _data[3] = 0xB8;
            _data[4] = 0x85;
            _data[5] = 0xA3;
            _data[6] = 0x00;
            _data[7] = 0x00;
            _data[8] = 0x00;
            _data[9] = 0x00;
            _data[10] = 0x8A;
            _data[11] = 0x2E;
            _data[12] = 0x03;
            _data[13] = 0x70;
            _data[14] = 0x73;
            _data[15] = 0x34;

            _reader = new RecordReader(_data);
        }

        private readonly byte[] _data = new byte[16];
        private readonly RecordReader _reader;

        [Fact]
        public void TestRecordAddress()
        {
            var a = new RecordAaaa(_reader);

            a.Address.ShouldBe(IPAddress.Parse("2001:0db8:85a3:0000:0000:8a2e:0370:7334"));
        }

        [Fact]
        public void TestRecordToString()
        {
            var a = new RecordAaaa(_reader);

            a.ToString().ShouldBe("2001:db8:85a3::8a2e:370:7334");
        }
    }
}