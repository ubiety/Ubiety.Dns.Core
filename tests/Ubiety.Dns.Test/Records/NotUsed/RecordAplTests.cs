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
            data = new byte[4];
            data[0] = 0;
            data[1] = 2;
            data[2] = 4;
            data[3] = 5;

            reader = new RecordReader(data, 2);
        }

        private readonly byte[] data;
        private readonly RecordReader reader;

        [Fact]
        public void TestRecordData()
        {
            var apl = new RecordApl(reader);

            apl.RecordData[0].ShouldBe(data[2]);
        }

        [Fact]
        public void TestToString()
        {
            var apl = new RecordApl(reader);

            apl.ToString().ShouldMatch("RecordApl is not-used");
        }
    }
}