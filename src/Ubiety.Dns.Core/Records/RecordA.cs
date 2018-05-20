using System;
using System.Globalization;
using System.Net;

/*
3.4.1. A RDATA format

+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
|                    ADDRESS                    |
+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

ADDRESS         A 32 bit Internet address.

Hosts that have multiple Internet addresses will have multiple A
records.
*
*/
namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     IPv4 Address DNS record
    /// </summary>
    public class RecordA : Record
    {
        private readonly String address;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordA" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordA(RecordReader rr)
        {
            this.address = $"{rr.ReadByte()}.{rr.ReadByte()}.{rr.ReadByte()}.{rr.ReadByte()}";
        }

        /// <summary>
        ///     Gets the IP address
        /// </summary>
        public IPAddress Address { get => IPAddress.Parse(this.address); }

        /// <summary>
        ///     String representation of the address
        /// </summary>
        /// <returns>String of the IP address</returns>
        public override String ToString()
        {
            return this.address;
        }
    }
}
