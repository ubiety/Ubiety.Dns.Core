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
    public record RecordAaaa : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAaaa" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordAaaa(RecordReader reader)
            : base(reader)
        {
            Address = IPAddress.Parse(
                $"{Reader.ReadUInt16():x4}:{Reader.ReadUInt16():x4}:{Reader.ReadUInt16():x4}:{Reader.ReadUInt16():x4}:{Reader.ReadUInt16():x4}:{Reader.ReadUInt16():x4}:{Reader.ReadUInt16():x4}:{Reader.ReadUInt16():x4}");
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
