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

using System.Globalization;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNS signature record.
    /// </summary>
    public record RecordSig : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSig" /> class.
        /// </summary>
        /// <param name="reader">Record reader for the record data.</param>
        public RecordSig(RecordReader reader)
            : base(reader)
        {
            TypeCovered = Reader.ReadUInt16();
            Algorithm = Reader.ReadByte();
            Labels = Reader.ReadByte();
            OriginalTTL = Reader.ReadUInt32();
            SignatureExpiration = Reader.ReadUInt32();
            SignatureInception = Reader.ReadUInt32();
            KeyTag = Reader.ReadUInt16();
            SignersName = Reader.ReadDomainName();
            Signature = Reader.ReadString();
        }

        /// <summary>
        ///     Gets or sets the type covered.
        /// </summary>
        public ushort TypeCovered { get; set; }

        /// <summary>
        ///     Gets or sets the signature algorithm.
        /// </summary>
        public byte Algorithm { get; set; }

        /// <summary>
        ///     Gets or sets the labels.
        /// </summary>
        public byte Labels { get; set; }

        /// <summary>
        ///     Gets or sets the original TTL.
        /// </summary>
        public uint OriginalTTL { get; set; }

        /// <summary>
        ///     Gets or sets the signature expiration.
        /// </summary>
        public uint SignatureExpiration { get; set; }

        /// <summary>
        ///     Gets or sets the signature inception.
        /// </summary>
        public uint SignatureInception { get; set; }

        /// <summary>
        ///     Gets or sets the key tag.
        /// </summary>
        public ushort KeyTag { get; set; }

        /// <summary>
        ///     Gets or sets the signers name.
        /// </summary>
        public string SignersName { get; set; }

        /// <summary>
        ///     Gets or sets the signature.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        ///     Get a string version of the record.
        /// </summary>
        /// <returns>String of the record.</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4} {5} {6} {7} \"{8}\"",
                TypeCovered,
                Algorithm,
                Labels,
                OriginalTTL,
                SignatureExpiration,
                SignatureInception,
                KeyTag,
                SignersName,
                Signature);
        }
    }
}
