using System;
using System.Globalization;
using System.Net;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    /// IPv4 Address DNS record
    /// </summary>
    /// <remarks>
    /// # [Description](#tab/description)
    /// A Records are the most basic type of DNS record and are used to point
    /// a domain or subdomain to an IP address.
    ///
    /// # [RFC](#tab/rfc)
    /// ```   
    /// A RDATA format
    ///
    /// +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /// |                    ADDRESS                    |
    /// +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///
    /// where:
    ///
    /// ADDRESS     A 32 bit internet address
    ///
    /// Hosts that have multiple internet address will have multiple A
    /// records.
    /// ```
    /// </remarks>
    public class RecordA : Record
    {
        private readonly String address;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordA" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data</param>
        public RecordA(RecordReader reader)
        {
            this.address = $"{reader.ReadByte()}.{reader.ReadByte()}.{reader.ReadByte()}.{reader.ReadByte()}";
        }

        /// <summary>
        /// Gets the IP address
        /// </summary>
        /// <value>IP address of the A record</value>
        public IPAddress Address { get => IPAddress.Parse(this.address); }

        /// <summary>
        /// String representation of the address
        /// </summary>
        /// <returns>String of the IP address</returns>
        public override String ToString()
        {
            return this.address;
        }
    }
}
