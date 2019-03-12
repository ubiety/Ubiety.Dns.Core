/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

/*
 *
3.3.1. CNAME RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     CNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

CNAME           A <domain-name> which specifies the canonical or primary
                name for the owner.  The owner name is an alias.

CNAME RRs cause no additional section processing, but name servers may
choose to restart the query at the canonical name in certain cases.  See
the description of name server logic in [RFC-1034] for details.

 *
 */
namespace Ubiety.Dns.Core.Records.General
{
    /// <summary>
    ///     Canonical name DNS record.
    /// </summary>
    public class RecordCname : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordCname" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordCname(RecordReader rr)
        {
            Cname = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets the canonical name.
        /// </summary>
        public string Cname { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String version of the cname.</returns>
        public override string ToString()
        {
            return Cname;
        }
    }
}