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

using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNS AFSDB Record.
    /// </summary>
    /// <remarks>
    ///     # [Description](#tab/description)
    ///     AFS Database location resource record
    ///     # [RFC](#tab/rfc)
    ///     This section defines an extension of the DNS to locate servers both
    ///     for AFS (AFS is a registered trademark of Transarc Corporation) and
    ///     for the Open Source Foundation's (OSF) Distributed Computing
    ///     Environment (DCE) authenticated naming system using HP/Apollo's NCA,
    ///     both to be components of the OSF DCE. The discussion assumes that
    ///     the reader is familiar with AFS [5] and NCA [6].
    ///     The AFS (originally the Andrew File System) system uses the DNS to
    ///     map from a domain name to the name of an AFS cell database server.
    ///     The DCE Naming service uses the DNS for a similar function: mapping
    ///     from the domain name of a cell to authenticated name servers for that
    ///     cell. The method uses a new RR type with mnemonic AFSDB and type
    ///     code of 18 (decimal).
    ///     AFSDB has the following format:
    ///     [owner] [ttl] [class] AFSDB [subtype] [hostname]
    ///     Both RDATA fields are required in all AFSDB RRs. The [subtype] field
    ///     is a 16 bit integer. The [hostname] field is a domain name of a host
    ///     that has a server for the cell named by the owner name of the RR.
    /// </remarks>
    public class RecordAfsdb : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAfsdb" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordAfsdb(RecordReader rr)
        {
            rr = rr.ThrowIfNull(nameof(rr));
            SubType = rr.ReadUInt16();
            Hostname = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets the subtype.
        /// </summary>
        /// <value>AFSDB subtype as an unsigned short.</value>
        public ushort SubType { get; }

        /// <summary>
        ///     Gets the hostname.
        /// </summary>
        /// <value>AFSDB hostname as a string.</value>
        public string Hostname { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String version of the data.</returns>
        public override string ToString()
        {
            return $"{SubType}{Hostname}";
        }
    }
}