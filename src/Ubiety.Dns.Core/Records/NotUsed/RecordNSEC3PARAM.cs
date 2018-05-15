namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     NSEC3PARAM DNS record
    /// </summary>
    public class RecordNsec3Param : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNsec3Param" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordNsec3Param(RecordReader rr)
            : base(rr)
        {
        }
    }
}
