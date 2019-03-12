/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     DNS public key resource record.
    /// </summary>
    public class RecordDnsKey : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordDnsKey" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordDnsKey(RecordReader rr)
            : base(rr)
        {
        }
    }
}