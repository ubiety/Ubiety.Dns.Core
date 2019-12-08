/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System.Collections.Generic;
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
    ///     NXT DNS Record.
    /// </summary>
    public class RecordNxt : Record
    {
        private readonly byte[] _bitmap;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNxt" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordNxt(RecordReader rr)
        {
            var reader = rr.ThrowIfNull(nameof(rr));
            var length = reader.ReadUInt16(-2);
            NextDomainName = reader.ReadDomainName();
            length -= (ushort)reader.Position;
            _bitmap = new byte[length];
            _bitmap = reader.ReadBytes(length);
        }

        /// <summary>
        ///     Gets the next domain name.
        /// </summary>
        public string NextDomainName { get; }

        /// <summary>
        ///     Gets the record bitmap.
        /// </summary>
        public List<byte> Bitmap => new List<byte>(_bitmap);

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String version of the data.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var bitNr = 1; bitNr < _bitmap.Length * 8; bitNr++)
            {
                if (IsSet(bitNr))
                {
                    sb.Append(" " + (RecordType)bitNr);
                }
            }

            return $"{NextDomainName}{sb}";
        }

        private bool IsSet(int bitNr)
        {
            var intByte = bitNr / 8;
            var intOffset = bitNr % 8;
            var b = _bitmap[intByte];
            var intTest = 1 << intOffset;
            return (b & intTest) != 0;
        }
    }
}