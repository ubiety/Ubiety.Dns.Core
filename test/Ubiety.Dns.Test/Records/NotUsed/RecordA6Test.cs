using System;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Records.NotUsed;
using Xunit;
using Xunit.Abstractions;

namespace Ubiety.Dns.Test.Records.NotUsed
{
    public class RecordA6Test
    {
        private readonly Byte[] data;
        private readonly RecordReader reader;

        private ITestOutputHelper output;

        public RecordA6Test(ITestOutputHelper output)
        {
            data = new Byte[4];
            data[0] = 0;
            data[1] = 2;
            data[2] = 4;
            data[3] = 5;

            this.output = output;

            reader = new RecordReader(data, 2);
        }

        [Fact]
        public void TestRecordData()
        {
            var a6 = new RecordA6(reader);

            foreach(var item in a6.RecordData)
            {
                output.WriteLine(item.ToString());
            }

            Assert.Equal(data[2], a6.RecordData[0]);
        }

        [Fact]
        public void TestToString()
        {
            var a6 = new RecordA6(reader);

            Assert.Equal("RecordA6 is not-used", a6.ToString());
        }
    }
}