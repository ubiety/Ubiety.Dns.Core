using System;
using System.Collections.Generic;
using System.Globalization;

/*
 * http://www.ietf.org/rfc/rfc2845.txt
 *
 * Field Name       Data Type      Notes
      --------------------------------------------------------------
      Algorithm Name   domain-name    Name of the algorithm
                                      in domain name syntax.
      Time Signed      u_int48_t      seconds since 1-Jan-70 UTC.
      Fudge            u_int16_t      seconds of error permitted
                                      in Time Signed.
      MAC Size         u_int16_t      number of octets in MAC.
      MAC              octet stream   defined by Algorithm Name.
      Original ID      u_int16_t      original message ID
      Error            u_int16_t      expanded RCODE covering
                                      TSIG processing.
      Other Len        u_int16_t      length, in octets, of
                                      Other Data.
      Other Data       octet stream   empty unless Error == BADTIME

 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Transaction signature DNS record
    /// </summary>
    public class RecordTsig : Record
    {
        private Byte[] mac;
        private Byte[] otherData;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordTsig" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordTsig(RecordReader rr)
        {
            this.AlgorithmName = rr.ReadDomainName();
            this.TimeSigned = rr.ReadUInt32() << 32 | rr.ReadUInt32();
            this.Fudge = rr.ReadUInt16();
            this.MacSize = rr.ReadUInt16();
            this.mac = rr.ReadBytes(this.MacSize);
            this.OriginalId = rr.ReadUInt16();
            this.Error = rr.ReadUInt16();
            this.OtherLength = rr.ReadUInt16();
            this.otherData = rr.ReadBytes(this.OtherLength);
        }

        /// <summary>
        ///     Gets or sets the algorithm name
        /// </summary>
        public String AlgorithmName { get; set; }

        /// <summary>
        ///     Gets or sets the time signed
        /// </summary>
        public Int64 TimeSigned { get; set; }

        /// <summary>
        ///     Gets or sets the number of seconds of error
        /// </summary>
        public UInt16 Fudge { get; set; }

        /// <summary>
        ///     Gets or sets the MAC size
        /// </summary>
        public UInt16 MacSize { get; set; }

        /// <summary>
        ///     Gets the MAC
        /// </summary>
        public List<Byte> Mac { get => new List<Byte>(this.mac); }

        /// <summary>
        ///     Gets or sets the original id
        /// </summary>
        public UInt16 OriginalId { get; set; }

        /// <summary>
        ///     Gets or sets the record error
        /// </summary>
        public UInt16 Error { get; set; }

        /// <summary>
        ///     Gets or sets the length of other data
        /// </summary>
        public UInt16 OtherLength { get; set; }

        /// <summary>
        ///     Gets the other record data
        /// </summary>
        public List<Byte> OtherData { get => new List<Byte>(this.otherData); }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>Signature as a string</returns>
        public override String ToString()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(this.TimeSigned);
            String printDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4}",
                this.AlgorithmName,
                printDate,
                this.Fudge,
                this.OriginalId,
                this.Error);
        }
    }
}
