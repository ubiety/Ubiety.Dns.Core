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
