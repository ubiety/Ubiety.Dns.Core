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

using System;
using System.Collections.Generic;

/*

 CERT RR
 *                     1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |             type              |             key tag           |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |   algorithm   |                                               /
   +---------------+            certificate or CRL                 /
   /                                                               /
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Certificate DNS record.
    /// </summary>
    public record RecordCert : Record
    {
        private readonly byte[] _rawKey;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordCert" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordCert(RecordReader reader)
            : base(reader)
        {
            // re-read length
            var recordLength = Reader.ReadUInt16(-2);

            Type = Reader.ReadUInt16();
            KeyTag = Reader.ReadUInt16();
            Algorithm = Reader.ReadByte();
            var length = recordLength - 5;
            _rawKey = Reader.ReadBytes(length);
            PublicKey = Convert.ToBase64String(_rawKey);
        }

        /// <summary>
        ///     Gets the record type.
        /// </summary>
        public ushort Type { get; }

        /// <summary>
        ///     Gets the key tag.
        /// </summary>
        public ushort KeyTag { get; }

        /// <summary>
        ///     Gets the algorithm.
        /// </summary>
        public byte Algorithm { get; }

        /// <summary>
        ///     Gets the public key.
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        ///     Gets the raw key.
        /// </summary>
        public List<byte> RawKey => new List<byte>(_rawKey);

        /// <summary>
        ///     String version of the record.
        /// </summary>
        /// <returns>String of the public key.</returns>
        public override string ToString()
        {
            return PublicKey;
        }
    }
}
