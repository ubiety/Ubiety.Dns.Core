namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Host Identity Protocol record
    /// </summary>
    public class RecordHip : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordHip" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordHip(RecordReader rr)
            : base(rr)
        {
        }
    }
}