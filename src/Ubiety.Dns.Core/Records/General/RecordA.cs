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
