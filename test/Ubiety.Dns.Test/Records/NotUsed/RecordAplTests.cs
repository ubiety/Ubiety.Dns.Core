using System;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records.NotUsed;
using Xunit;

namespace Ubiety.Dns.Test.Records.NotUsed
{
    public class RecordAplTests
    {
        private readonly Byte[] data;
        private readonly RecordReader reader;

        public RecordAplTests()
        {
            data = new Byte[4];
            data[0] = 0;
            data[1] = 2;
            data[2] = 4;
            data[3] = 5;

            reader = new RecordReader(data, 2);
        }

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