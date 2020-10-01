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
