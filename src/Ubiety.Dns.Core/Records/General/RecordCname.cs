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
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordCname(RecordReader reader)
            : base(reader)
        {
            Cname = Reader.ReadDomainName();
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