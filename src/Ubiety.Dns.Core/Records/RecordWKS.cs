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

using System.Collections.ObjectModel;
using System.Globalization;
using Ubiety.Dns.Core.Common;

/*
* 3.4.2. WKS RDATA format

+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
|                    ADDRESS                    |
+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
|       PROTOCOL        |                       |
+--+--+--+--+--+--+--+--+                       |
|                                               |
/                   <BIT MAP>                   /
/                                               /
+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

ADDRESS         An 32 bit Internet address

PROTOCOL        An 8 bit IP protocol number

<BIT MAP>       A variable length bit map.  The bit map must be a
multiple of 8 bits long.

The WKS record is used to describe the well known services supported by
a particular protocol on a particular internet address.  The PROTOCOL
field specifies an IP protocol number, and the bit map has one bit per
port of the specified protocol.  The first bit corresponds to port 0,
the second to port 1, etc.  If the bit map does not include a bit for a
protocol of interest, that bit is assumed zero.  The appropriate values
and mnemonics for ports and protocols are specified in [RFC-1010].

For example, if PROTOCOL=TCP (6), the 26th bit corresponds to TCP port
25 (SMTP).  If this bit is set, a SMTP server should be listening on TCP
port 25; if zero, SMTP service is not supported on the specified
address.

The purpose of WKS RRs is to provide availability information for
servers for TCP and UDP.  If a server supports both TCP and UDP, or has
multiple Internet addresses, then multiple WKS RRs are used.

WKS RRs cause no additional section processing.

In master files, both ports and protocols are expressed using mnemonics
or decimal numbers.

*/
namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNS well known services record.
    /// </summary>
    public class RecordWks : Record
    {
        private readonly byte[] _bitmap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordWks" /> class.
        /// </summary>
        /// <param name="rr">Record reader for record data.</param>
        public RecordWks(RecordReader rr)
        {
            rr = rr.ThrowIfNull(nameof(rr));
            var length = rr.ReadUInt16(-2);
            Address = string.Format(
                CultureInfo.InvariantCulture,
                "{0}.{1}.{2}.{3}",
                rr.ReadByte(),
                rr.ReadByte(),
                rr.ReadByte(),
                rr.ReadByte());
            Protocol = rr.ReadByte();
            length -= 5;
            _bitmap = new byte[length];
            _bitmap = rr.ReadBytes(length);
        }

        /// <summary>
        ///     Gets or sets the address of the server.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Gets or sets the protocol of the service.
        /// </summary>
        public int Protocol { get; set; }

        /// <summary>
        ///     Gets the service bitmap.
        /// </summary>
        public Collection<byte> Bitmap => new Collection<byte>(_bitmap);

        /// <summary>
        ///     Return a string of the well known service record.
        /// </summary>
        /// <returns>String of the record.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", Address, Protocol);
        }
    }
}