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
using System.Globalization;

/*
* http://tools.ietf.org/rfc/rfc2930.txt
*
2. The TKEY Resource Record

The TKEY resource record (RR) has the structure given below.  Its RR
type code is 249.

Field       Type         Comment
-----       ----         -------
Algorithm:   domain
Inception:   u_int32_t
Expiration:  u_int32_t
Mode:        u_int16_t
Error:       u_int16_t
Key Size:    u_int16_t
Key Data:    octet-stream
Other Size:  u_int16_t
Other Data:  octet-stream  undefined by this specification

*/

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Transaction key DNS resource record.
    /// </summary>
    public record RecordTkey : Record
    {
        private readonly byte[] _keyData;
        private readonly byte[] _otherData;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordTkey" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordTkey(RecordReader reader)
            : base(reader)
        {
            Algorithm = Reader.ReadDomainName();
            Inception = Reader.ReadUInt32();
            Expiration = Reader.ReadUInt32();
            Mode = Reader.ReadUInt16();
            Error = Reader.ReadUInt16();
            KeySize = Reader.ReadUInt16();
            _keyData = Reader.ReadBytes(KeySize);
            OtherSize = Reader.ReadUInt16();
            _otherData = Reader.ReadBytes(OtherSize);
        }

        /// <summary>
        ///     Gets the key algorithm.
        /// </summary>
        public string Algorithm { get; }

        /// <summary>
        ///     Gets the inception time of the key.
        /// </summary>
        public uint Inception { get; }

        /// <summary>
        ///     Gets the expiration time of the key.
        /// </summary>
        public uint Expiration { get; }

        /// <summary>
        ///     Gets the key agreement mode.
        /// </summary>
        public ushort Mode { get; }

        /// <summary>
        ///     Gets the error code of the record.
        /// </summary>
        public ushort Error { get; }

        /// <summary>
        ///     Gets the key size from the record.
        /// </summary>
        public ushort KeySize { get; }

        /// <summary>
        ///     Gets the key data.
        /// </summary>
        public List<byte> KeyData => new(_keyData);

        /// <summary>
        ///     Gets the other size from the record (Future use).
        /// </summary>
        public ushort OtherSize { get; }

        /// <summary>
        ///     Gets the other data from the record (Future use).
        /// </summary>
        public List<byte> OtherData => new(_otherData);

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Key data as a string.</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4}",
                Algorithm,
                Inception,
                Expiration,
                Mode,
                Error);
        }
    }
}
