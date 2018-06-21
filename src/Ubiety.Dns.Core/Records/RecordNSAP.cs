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
        private readonly byte[] address;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNsap" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordNsap(RecordReader rr)
        {
            Length = rr.ReadUInt16();
            address = rr.ReadBytes(Length);
        }

        /// <summary>
        ///     Gets or sets the length
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        ///     Gets the address as a byte collection
        /// </summary>
        public Collection<byte> NsapAddress => new Collection<byte>(address);

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>NSAP address as a string</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} ", Length);
            for (var i = 0; i < address.Length; i++)
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X00}", address[i]);

            return sb.ToString();
        }

        /// <summary>
        ///     Converts the address to a readable string
        /// </summary>
        /// <returns>String of the address in IPv2 format</returns>
        public string ToGOSIPV2()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:X}.{1:X}.{2:X}.{3:X}.{4:X}.{5:X}.{6:X}{7:X}.{8:X}",
                address[0],
                (address[1] << 8) | address[2],
                address[3],
                (address[4] << 16) | (address[5] << 8) | address[6],
                (address[7] << 8) | address[8],
                (address[9] << 8) | address[10],
                (address[11] << 8) | address[12],
                (address[13] << 16) | (address[14] << 8) | address[15],
                (address[16] << 16) | (address[17] << 8) | address[18]);
        }
    }
}