/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     DNS entity identifier resource record.
    /// </summary>
    public class RecordEid : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordEid" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordEid(RecordReader rr)
            : base(rr)
        {
        }
    }
}