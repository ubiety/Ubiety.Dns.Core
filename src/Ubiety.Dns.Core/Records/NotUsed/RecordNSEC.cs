namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     NSEC DNS record
    /// </summary>
    public class RecordNsec : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNsec" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordNsec(RecordReader rr)
            : base(rr)
        {
        }
    }
}
