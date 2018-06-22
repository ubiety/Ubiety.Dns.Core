using System;
using System.Collections.Generic;

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
        private readonly Byte[] _data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNull" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordNull(RecordReader rr)
        {
            rr.Position -= 2;
            var recordLength = rr.ReadUInt16();
            _data = new Byte[recordLength];
            _data = rr.ReadBytes(recordLength);
        }

        /// <summary>
        ///     Gets the record data
        /// </summary>
        public List<Byte> Data => new List<Byte>(_data);

        /// <summary>
        ///     String representation of the data
        /// </summary>
        /// <returns>Record data as a string</returns>
        public override String ToString()
        {
            return $"...binary data... ({_data.Length}) bytes";
        }
    }
}