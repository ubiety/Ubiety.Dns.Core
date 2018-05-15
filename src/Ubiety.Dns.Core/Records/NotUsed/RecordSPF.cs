namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Sender policy framework
    /// </summary>
    public class RecordSpf : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSpf" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordSpf(RecordReader rr)
            : base(rr)
        {
        }
    }
}
