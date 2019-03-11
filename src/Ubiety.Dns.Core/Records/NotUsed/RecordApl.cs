namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Experimental address prefix list resource record.
    /// </summary>
    public class RecordApl : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordApl" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordApl(RecordReader rr)
            : base(rr)
        {
        }
    }
}