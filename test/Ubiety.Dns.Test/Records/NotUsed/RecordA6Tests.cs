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
            var a6 = new RecordA6(reader);

            a6.RecordData[0].ShouldBe(data[2]);
        }

        [Fact]
        public void TestToString()
        {
            var a6 = new RecordA6(reader);

            a6.ToString().ShouldMatch("RecordA6 is not-used");
        }
    }
}