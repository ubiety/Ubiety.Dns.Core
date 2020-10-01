/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
    ///     Null DNS record.
    /// </summary>
    public record RecordNull : Record
    {
        private readonly byte[] _data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNull" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordNull(RecordReader reader)
            : base(reader)
        {
            Reader.Position -= 2;
            var recordLength = Reader.ReadUInt16();
            _data = new byte[recordLength];
            _data = Reader.ReadBytes(recordLength);
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
