/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
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