/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

/*
 3.3.11. NS RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   NSDNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

NSDNAME         A <domain-name> which specifies a host which should be
                authoritative for the specified class and domain.

NS records cause both the usual additional section processing to locate
a type A record, and, when used in a referral, a special search of the
zone in which they reside for glue information.

The NS RR states that the named host should be expected to have a zone
starting at owner name of the specified class.  Note that the class may
not indicate the protocol family which should be used to communicate
with the host, although it is typically a strong hint.  For example,
hosts which are name servers for either Internet (IN) or Hesiod (HS)
class information are normally queried using IN class protocols.
 */

using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Nameserver DNS record.
    /// </summary>
    public class RecordNs : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNs" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordNs(RecordReader rr)
        {
            NameserverDomain = rr.ThrowIfNull(nameof(rr)).ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the nameserver domain.
        /// </summary>
        public string NameserverDomain { get; set; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Nameserver domain as a string.</returns>
        public override string ToString()
        {
            return NameserverDomain;
        }
    }
}