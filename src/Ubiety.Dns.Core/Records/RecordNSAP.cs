using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

/*
 * http://tools.ietf.org/rfc/rfc1348.txt
 * http://tools.ietf.org/html/rfc1706
 *
 *              |--------------|
              | <-- IDP -->  |
              |--------------|-------------------------------------|
              | AFI |  IDI   |            <-- DSP -->              |
              |-----|--------|-------------------------------------|
              | 47  |  0005  | DFI | AA |Rsvd | RD |Area | ID |Sel |
              |-----|--------|-----|----|-----|----|-----|----|----|
       octets |  1  |   2    |  1  | 3  |  2  | 2  |  2  | 6  | 1  |
              |-----|--------|-----|----|-----|----|-----|----|----|

                    IDP    Initial Domain Part
                    AFI    Authority and Format Identifier
                    IDI    Initial Domain Identifier
                    DSP    Domain Specific Part
                    DFI    DSP Format Identifier
                    AA     Administrative Authority
                    Rsvd   Reserved
                    RD     Routing Domain Identifier
                    Area   Area Identifier
                    ID     System Identifier
                    SEL    NSAP Selector

                  Figure 1: GOSIP Version 2 NSAP structure.

 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Network service access point DNS record
    /// </summary>
    public class RecordNsap : Record
    {
        private readonly Byte[] address;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNsap" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordNsap(RecordReader rr)
        {
            this.Length = rr.ReadUInt16();
            this.address = rr.ReadBytes(this.Length);
        }

        /// <summary>
        ///     Gets or sets the length
        /// </summary>
        public UInt16 Length { get; set; }

        /// <summary>
        ///     Gets the address as a byte collection
        /// </summary>
        public Collection<Byte> NsapAddress { get => new Collection<Byte>(this.address); }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>NSAP address as a string</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} ", this.Length);
            for (Int32 i = 0; i < this.address.Length; i++)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X00}", this.address[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Converts the address to a readable string
        /// </summary>
        /// <returns>String of the address in IPv2 format</returns>
        public String ToGOSIPV2()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:X}.{1:X}.{2:X}.{3:X}.{4:X}.{5:X}.{6:X}{7:X}.{8:X}",
                this.address[0],
                this.address[1] << 8 | this.address[2],
                this.address[3],
                this.address[4] << 16 | this.address[5] << 8 | this.address[6],
                this.address[7] << 8 | this.address[8],
                this.address[9] << 8 | this.address[10],
                this.address[11] << 8 | this.address[12],
                this.address[13] << 16 | this.address[14] << 8 | this.address[15],
                this.address[16] << 16 | this.address[17] << 8 | this.address[18]);
        }
    }
}
