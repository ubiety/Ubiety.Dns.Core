namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     UNSPEC DNS record
    /// </summary>
    public class RecordUnspec : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordUnspec" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordUnspec(RecordReader rr)
            : base(rr)
        {
        }
    }
}
