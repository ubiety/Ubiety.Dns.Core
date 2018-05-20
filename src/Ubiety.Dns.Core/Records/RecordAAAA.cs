using System;
using System.Globalization;
using System.Net;

/*
2.2 AAAA data format

   A 128 bit IPv6 address is encoded in the data portion of an AAAA
   resource record in network byte order (high-order byte first).
 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     IPv6 Address record
    /// </summary>
    public class RecordAaaa : Record
    {
        private readonly IPAddress address;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAaaa" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordAaaa(RecordReader rr)
        {
            this.address = IPAddress.Parse(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0:x}:{1:x}:{2:x}:{3:x}:{4:x}:{5:x}:{6:x}:{7:x}",
                    rr.ReadUInt16(),
                    rr.ReadUInt16(),
                    rr.ReadUInt16(),
                    rr.ReadUInt16(),
                    rr.ReadUInt16(),
                    rr.ReadUInt16(),
                    rr.ReadUInt16(),
                    rr.ReadUInt16()));
        }

        /// <summary>
        ///     Gets the IP address of the record
        /// </summary>
        public IPAddress Address { get => this.address; }

        /// <summary>
        ///     String version of the record
        /// </summary>
        /// <returns>String of the address</returns>
        public override string ToString()
        {
            return this.address.ToString();
        }
    }
}
