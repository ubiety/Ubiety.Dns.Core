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

using System;
using System.Net;

namespace Ubiety.Dns.Core.Records.General
{
    /// <summary>
    ///     IPv6 Address record.
    /// </summary>
    /// <remarks>
    ///     # [Description](#tab/description)
    ///     The AAAA resource record type is a record specific to the Internet
    ///     class that stores a single IPv6 address
    ///     # [RFC](#tab/rfc)
    ///     ```
    ///     A 128 bit IPv6 address is encoded in the data portion of an AAAA
    ///     resource record in network byte order (high-order byte first)
    ///     ```.
    /// </remarks>
    public class RecordAaaa : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAaaa" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordAaaa(RecordReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            Address = IPAddress.Parse(
                $"{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}");
        }

        /// <summary>
        ///     Gets the IP address of the record.
        /// </summary>
        /// <value>IP address of the AAAA record.</value>
        public IPAddress Address { get; }

        /// <summary>
        ///     String version of the record.
        /// </summary>
        /// <returns>String of the address.</returns>
        public override string ToString()
        {
            return Address.ToString();
        }
    }
}