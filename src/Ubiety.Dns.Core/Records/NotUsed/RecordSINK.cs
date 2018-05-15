namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     SINK DNS Record
    /// </summary>
    public class RecordSink : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSink" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordSink(RecordReader rr)
            : base(rr)
        {
        }
    }
}
