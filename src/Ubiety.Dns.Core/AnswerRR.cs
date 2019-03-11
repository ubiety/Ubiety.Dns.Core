namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     Answer resource record.
    /// </summary>
    public class AnswerRR : ResourceRecord
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AnswerRR" /> class.
        /// </summary>
        /// <param name="br"><see cref="RecordReader" /> for the record data.</param>
        public AnswerRR(RecordReader br)
            : base(br)
        {
        }
    }
}