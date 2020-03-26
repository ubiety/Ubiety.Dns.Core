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
using System.Globalization;
using Ubiety.Dns.Core.Common;

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
    public class RecordTkey : Record
    {
        private readonly byte[] _keyData;
        private readonly byte[] _otherData;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordTkey" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordTkey(RecordReader rr)
        {
            rr = rr.ThrowIfNull(nameof(rr));
            Algorithm = rr.ReadDomainName();
            Inception = rr.ReadUInt32();
            Expiration = rr.ReadUInt32();
            Mode = rr.ReadUInt16();
            Error = rr.ReadUInt16();
            KeySize = rr.ReadUInt16();
            _keyData = rr.ReadBytes(KeySize);
            OtherSize = rr.ReadUInt16();
            _otherData = rr.ReadBytes(OtherSize);
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
        public List<byte> KeyData => new List<byte>(_keyData);

        /// <summary>
        ///     Gets the other size from the record (Future use).
        /// </summary>
        public ushort OtherSize { get; }

        /// <summary>
        ///     Gets the other data from the record (Future use).
        /// </summary>
        public List<byte> OtherData => new List<byte>(_otherData);

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