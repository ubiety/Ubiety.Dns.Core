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

/* http://tools.ietf.org/rfc/rfc1183.txt

3.1. The X25 RR

   The X25 RR is defined with mnemonic X25 and type code 19 (decimal).

   X25 has the following format:

   <owner> <ttl> <class> X25 <PSDN-address>

   <PSDN-address> is required in all X25 RRs.

   <PSDN-address> identifies the PSDN (Public Switched Data Network)
   address in the X.121 [10] numbering plan associated with <owner>.
   Its format in master files is a <character-string> syntactically
   identical to that used in TXT and HINFO.

   The format of X25 is class insensitive.  X25 RRs cause no additional
   section processing.

   The <PSDN-address> is a string of decimal digits, beginning with the
   4 digit DNIC (Data Network Identification Code), as specified in
   X.121. National prefixes (such as a 0) MUST NOT be used.

   For example:

   Relay.Prime.COM.  X25       311061700956


 */

using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     X25 DNS record.
    /// </summary>
    public class RecordX25 : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordX25" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordX25(RecordReader rr)
        {
            PSDNAdress = rr.ThrowIfNull(nameof(rr)).ReadString();
        }

        /// <summary>
        ///     Gets or sets the PSDN address.
        /// </summary>
        public string PSDNAdress { get; set; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>PSDN address as a string.</returns>
        public override string ToString()
        {
            return PSDNAdress;
        }
    }
}