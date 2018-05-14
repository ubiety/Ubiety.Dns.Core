namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Historic IPv6 record lookup
    /// </summary>
    public class RecordA6 : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordA6" /> class
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data</param>
        public RecordA6(RecordReader reader)
            : base(reader)
        {
        }
    }
}
