/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     Additional resource record.
    /// </summary>
    public class AdditionalResourceRecord : ResourceRecord
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AdditionalResourceRecord" /> class.
        /// </summary>
        /// <param name="br"><see cref="ResourceRecord" /> for the record data.</param>
        public AdditionalResourceRecord(RecordReader br)
            : base(br)
        {
        }
    }
}