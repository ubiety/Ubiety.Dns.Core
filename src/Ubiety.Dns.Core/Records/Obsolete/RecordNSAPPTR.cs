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
 * http://tools.ietf.org/rfc/rfc1348.txt

 * The NSAP-PTR RR

   The NSAP-PTR RR is defined with mnemonic NSAP-PTR and a type code 23
   (decimal).

   Its function is analogous to the PTR record used for IP addresses [4,7].

   NSAP-PTR has the following format:

   <NSAP-suffix> <ttl> <class> NSAP-PTR <owner>

   All fields are required.

   <NSAP-suffix> enumerates the actual octet values assigned by the
   assigning authority for the LOCAL network.  Its format in master
   files is a <character-string> syntactically identical to that used in
   TXT and HINFO.

   The format of NSAP-PTR is class insensitive.  NSAP-PTR RR causes no
   additional section processing.

   For example:

   In net ff08000574.nsap-in-addr.arpa:

   444433332222111199990123000000ff    NSAP-PTR   foo.bar.com.

   Or in net 11110031f67293.nsap-in-addr.arpa:

   67894444333322220000  NSAP-PTR        host.school.de.

   The RR data is the ASCII representation of the digits.  It is encoded
   as a <character-string>.

 */

using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core.Records.Obsolete
{
    /// <summary>
    ///     NSAP PTR DNS Record.
    /// </summary>
    public class RecordNsapPtr : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNsapPtr" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordNsapPtr(RecordReader rr)
        {
            Owner = rr.ThrowIfNull(nameof(rr)).ReadString();
        }

        /// <summary>
        ///     Gets the owner.
        /// </summary>
        public string Owner { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String version of the data.</returns>
        public override string ToString()
        {
            return Owner;
        }
    }
}