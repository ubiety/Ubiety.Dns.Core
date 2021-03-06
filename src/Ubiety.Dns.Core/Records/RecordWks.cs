/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;

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
    public record RecordWks : Record
    {
        private readonly byte[] _bitmap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordWks" /> class.
        /// </summary>
        /// <param name="reader">Record reader for record data.</param>
        public RecordWks(RecordReader reader)
            : base(reader)
        {
            var length = Reader.ReadUInt16(-2);
            Address = $"{Reader.ReadByte()}.{Reader.ReadByte()}.{Reader.ReadByte()}.{Reader.ReadByte()}";
            Protocol = Reader.ReadByte();
            length -= 5;
            _bitmap = new byte[length];
            _bitmap = Reader.ReadBytes(length);
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
        public IEnumerable<byte> Bitmap => new List<byte>(_bitmap);

        /// <summary>
        ///     Return a string of the well known service record.
        /// </summary>
        /// <returns>String of the record.</returns>
        public override string ToString()
        {
            return $"{Address} {Protocol}";
        }
    }
}
