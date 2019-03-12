/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     Authority resource record.
    /// </summary>
    public class AuthorityRR : ResourceRecord
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorityRR" /> class.
        /// </summary>
        /// <param name="br"><see cref="ResourceRecord" /> for the record data.</param>
        public AuthorityRR(RecordReader br)
            : base(br)
        {
        }
    }
}