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

using System.Net;

namespace Ubiety.Dns.Core.Records.General
{
    /// <summary>
    ///     IPv4 Address DNS record.
    /// </summary>
    /// <remarks>
    ///     # [Description](#tab/description)
    ///     A Records are the most basic type of DNS record and are used to point
    ///     a domain or subdomain to an IP address.
    ///     # [RFC](#tab/rfc)
    ///     ```
    ///     A RDATA format
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     |                    ADDRESS                    |
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     where:
    ///     ADDRESS     A 32 bit internet address
    ///     Hosts that have multiple internet address will have multiple A
    ///     records.
    ///     ```.
    /// </remarks>
    public record RecordA : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordA" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordA(RecordReader reader)
            : base(reader)
        {
            Address = IPAddress.Parse($"{Reader.ReadByte()}.{Reader.ReadByte()}.{Reader.ReadByte()}.{Reader.ReadByte()}");
        }

        /// <summary>
        ///     Gets the IP address.
        /// </summary>
        /// <value>IP address of the A record.</value>
        public IPAddress Address { get; }

        /// <summary>
        ///     String representation of the address.
        /// </summary>
        /// <returns>String of the IP address.</returns>
        public override string ToString()
        {
            return Address.ToString();
        }
    }
}
