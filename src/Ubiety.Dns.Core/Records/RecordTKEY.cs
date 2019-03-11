/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
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