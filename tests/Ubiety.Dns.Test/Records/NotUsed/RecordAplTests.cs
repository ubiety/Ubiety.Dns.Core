using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records.NotUsed;
using Xunit;

namespace Ubiety.Dns.Test.Records.NotUsed
{
    public class RecordAplTests
    {
        public RecordAplTests()
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
            var apl = new RecordApl(_reader);

            apl.RecordData[0].ShouldBe(_data[2]);
        }

        [Fact]
        public void TestToString()
        {
            var apl = new RecordApl(_reader);

            apl.ToString().ShouldMatch("RecordApl is not-used");
        }
    }
}