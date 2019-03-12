/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Nimloc record.
    /// </summary>
    public class RecordNimloc : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNimloc" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordNimloc(RecordReader rr)
            : base(rr)
        {
        }
    }
}