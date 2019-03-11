namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     UID DNS record.
    /// </summary>
    public class RecordUid : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordUid" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordUid(RecordReader rr)
            : base(rr)
        {
        }
    }
}