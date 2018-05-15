namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     UINFO DNS record
    /// </summary>
    public class RecordUinfo : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordUinfo" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordUinfo(RecordReader rr)
            : base(rr)
        {
        }
    }
}
