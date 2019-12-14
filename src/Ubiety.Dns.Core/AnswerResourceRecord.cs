/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     Answer resource record.
    /// </summary>
    public class AnswerResourceRecord : ResourceRecord
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AnswerResourceRecord" /> class.
        /// </summary>
        /// <param name="br"><see cref="RecordReader" /> for the record data.</param>
        public AnswerResourceRecord(RecordReader br)
            : base(br)
        {
        }
    }
}