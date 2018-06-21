namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     IPSEC Key DNS record
    /// </summary>
    public class RecordIpsecKey : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordIpsecKey" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordIpsecKey(RecordReader rr)
            : base(rr)
        {
        }
    }
}