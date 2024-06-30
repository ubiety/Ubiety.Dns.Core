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
    ///     Network service access point DNS record.
    /// </summary>
    public record RecordNsap : Record
    {
        private readonly byte[] _address;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNsap" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordNsap(RecordReader reader)
            : base(reader)
        {
            Length = Reader.ReadUInt16();
            _address = Reader.ReadBytes(Length);
        }

        /// <summary>
        ///     Gets or sets the length.
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        ///     Gets the address as a byte collection.
        /// </summary>
        public Collection<byte> NsapAddress => new(_address);

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>NSAP address as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} ", Length);
            foreach (var t in _address)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X00}", t);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Converts the address to a readable string.
        /// </summary>
        /// <returns>String of the address in IPv2 format.</returns>
        public string ToGOSIPV2()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0:X}.{1:X}.{2:X}.{3:X}.{4:X}.{5:X}.{6:X}{7:X}.{8:X}",
                _address[0],
                (_address[1] << 8) | _address[2],
                _address[3],
                (_address[4] << 16) | (_address[5] << 8) | _address[6],
                (_address[7] << 8) | _address[8],
                (_address[9] << 8) | _address[10],
                (_address[11] << 8) | _address[12],
                (_address[13] << 16) | (_address[14] << 8) | _address[15],
                (_address[16] << 16) | (_address[17] << 8) | _address[18]);
        }
    }
}
