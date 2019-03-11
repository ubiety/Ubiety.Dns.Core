/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Resource record signature record.
    /// </summary>
    public class RecordRrsig : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordRrsig" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordRrsig(RecordReader rr)
            : base(rr)
        {
        }
    }
}