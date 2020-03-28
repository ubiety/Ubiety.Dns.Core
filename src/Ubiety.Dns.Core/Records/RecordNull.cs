/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using Ubiety.Dns.Core.Common.Extensions;

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
    ///     Null DNS record.
    /// </summary>
    public class RecordNull : Record
    {
        private readonly byte[] _data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNull" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordNull(RecordReader rr)
        {
            rr = rr.ThrowIfNull(nameof(rr));
            rr.Position -= 2;
            var recordLength = rr.ReadUInt16();
            _data = new byte[recordLength];
            _data = rr.ReadBytes(recordLength);
        }

        /// <summary>
        ///     Gets the record data.
        /// </summary>
        public List<byte> Data => new List<byte>(_data);

        /// <summary>
        ///     String representation of the data.
        /// </summary>
        /// <returns>Record data as a string.</returns>
        public override string ToString()
        {
            return $"...binary data... ({_data.Length}) bytes";
        }
    }
}