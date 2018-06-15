using System;
using System.Globalization;
using System.Net;

namespace Ubiety.Dns.Core.Records.General
{
    /// <summary>
    /// IPv6 Address record
    /// </summary>
    /// <remarks>
    /// # [Description](#tab/description)
    ///
    /// The AAAA resource record type is a record specific to the Internet
    /// class that stores a single IPv6 address
    ///
    /// # [RFC](#tab/rfc)
    ///
    /// ```
    /// A 128 bit IPv6 address is encoded in the data portion of an AAAA
    /// resource record in network byte order (high-order byte first)
    /// ```
    /// </remarks>
    public class RecordAaaa : Record
    {
        private readonly IPAddress address;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordAaaa" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data</param>
        public RecordAaaa(RecordReader reader)
        {
            this.address = IPAddress.Parse(
                    $"{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}:{reader.ReadUInt16():x4}");
        }

        /// <summary>
        /// Gets the IP address of the record
        /// </summary>
        /// <value>IP address of the AAAA record</value>
        public IPAddress Address { get => this.address; }

        /// <summary>
        /// String version of the record
        /// </summary>
        /// <returns>String of the address</returns>
        public override String ToString()
        {
            return this.address.ToString();
        }
    }
}
