namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     GID DNS record
    /// </summary>
    public class RecordGid : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordGid" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordGid(RecordReader rr)
            : base(rr)
        {
        }
    }
}
