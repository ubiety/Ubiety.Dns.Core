using System;
using System.Collections.ObjectModel;
using System.Text;
using Ubiety.Dns.Core.Common;

/*
 * http://tools.ietf.org/rfc/rfc2065.txt
 *
5.2 NXT RDATA Format

   The RDATA for an NXT RR consists simply of a domain name followed by
   a bit map.

   The type number for the NXT RR is 30.

                           1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
       0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |         next domain name                                      /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                    type bit map                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

   The NXT RR type bit map is one bit per RR type present for the owner
   name similar to the WKS socket bit map.  The first bit represents RR
   type zero (an illegal type which should not be present.) A one bit
   indicates that at least one RR of that type is present for the owner
   name.  A zero indicates that no such RR is present.  All bits not
   specified because they are beyond the end of the bit map are assumed
   to be zero.  Note that bit 30, for NXT, will always be on so the
   minimum bit map length is actually four octets.  The NXT bit map
   should be printed as a list of RR type mnemonics or decimal numbers
   similar to the WKS RR.

   The domain name may be compressed with standard DNS name compression
   when being transmitted over the network.  The size of the bit map can
   be inferred from the RDLENGTH and the length of the next domain name.



 */
namespace Ubiety.Dns.Core.Records.Obsolete
{
    /// <summary>
    ///     NXT DNS Record
    /// </summary>
    public class RecordNxt : Record
    {
        private Byte[] bitmap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNxt" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordNxt(RecordReader rr)
        {
            UInt16 length = rr.ReadUInt16(-2);
            this.NextDomainName = rr.ReadDomainName();
            length -= (UInt16)rr.Position;
            this.bitmap = new byte[length];
            this.bitmap = rr.ReadBytes(length);
        }

        /// <summary>
        ///     Gets or sets the next domain name
        /// </summary>
        public String NextDomainName { get; set; }

        /// <summary>
        ///     Gets the record bitmap
        /// </summary>
        public Collection<Byte> Bitmap { get => new Collection<Byte>(this.bitmap); }

        /// <summary>
        ///     String representation of the record
        /// </summary>
        /// <returns>String version of the data</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (Int32 bitNr = 1; bitNr < (this.bitmap.Length * 8); bitNr++)
            {
                if (this.IsSet(bitNr))
                {
                    sb.Append(" " + (RecordType)bitNr);
                }
            }

            return $"{this.NextDomainName}{sb.ToString()}";
        }

        private bool IsSet(Int32 bitNr)
        {
            Int32 intByte = (Int32)(bitNr / 8);
            Int32 intOffset = bitNr % 8;
            Byte b = this.bitmap[intByte];
            Int32 intTest = 1 << intOffset;
            if ((b & intTest) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
