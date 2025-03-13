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

namespace Ubiety.Dns.Core.Records.General;

/// <summary>
/// Represents the DNS AAAA (IPv6) resource record type.
/// </summary>
/// <remarks>
/// The AAAA resource record is used in the Internet class to store a single IPv6 address.
/// It encodes a 128-bit IPv6 address in network byte order (high-order byte first) within the data portion of the record.
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
    /// Gets the IPv6 address associated with this AAAA record.
    /// </summary>
    /// <remarks>
    /// This property stores a 128-bit IPv6 address in network byte order and represents the main data of the AAAA resource record.
    /// </remarks>
    public IPAddress Address { get; }

    /// <summary>
    /// Returns the string representation of the record's address.
    /// </summary>
    /// <returns>The string representation of the address.</returns>
    public override string ToString()
    {
        return Address.ToString();
    }
}