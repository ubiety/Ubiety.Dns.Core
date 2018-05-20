using System;
using System.Collections.ObjectModel;

/*
3.3.10. NULL RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                  <anything>                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

Anything at all may be in the RDATA field so long as it is 65535 octets
or less.

NULL records cause no additional section processing.  NULL RRs are not
allowed in master files.  NULLs are used as placeholders in some
experimental extensions of the DNS.
*/

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Null DNS record
    /// </summary>
    public class RecordNull : Record
    {
        private readonly Byte[] data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNull" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordNull(RecordReader rr)
        {
            rr.Position -= 2;
            UInt16 recordLength = rr.ReadUInt16();
            this.data = new Byte[recordLength];
            this.data = rr.ReadBytes(recordLength);
        }

        /// <summary>
        ///     Gets the record data
        /// </summary>
        public Collection<Byte> Data { get => new Collection<Byte>(this.data); }

        /// <summary>
        ///     String representation of the data
        /// </summary>
        /// <returns>Record data as a string</returns>
        public override String ToString()
        {
            return $"...binary data... ({this.data.Length}) bytes";
        }
    }
}
