using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records.NotUsed;
using Xunit;

namespace Ubiety.Dns.Test.Records.NotUsed
{
    public class RecordA6Tests
    {
        public RecordA6Tests()
        {
            _data = new byte[4];
            _data[0] = 0;
            _data[1] = 2;
            _data[2] = 4;
            _data[3] = 5;

            _reader = new RecordReader(_data, 2);
        }

        private readonly byte[] _data;
        private readonly RecordReader _reader;

        [Fact]
        public void TestRecordData()
        {
            var a6 = new RecordA6(_reader);

            a6.RecordData[0].ShouldBe(_data[2]);
        }

        [Fact]
        public void TestToString()
        {
            var a6 = new RecordA6(_reader);

            a6.ToString().ShouldMatch("RecordA6 is not-used");
        }
    }
}