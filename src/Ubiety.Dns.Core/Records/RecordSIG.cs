using System;
using System.Globalization;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNS signature record
    /// </summary>
    public class RecordSIG : Record
    {
        /// <summary>
        ///     Gets or sets the type covered
        /// </summary>
        public UInt16 TypeCovered { get; set; }

        /// <summary>
        ///     Gets or sets the signature algorithm
        /// </summary>
        public byte Algorithm { get; set; }

        /// <summary>
        ///     Gets or sets the labels
        /// </summary>
        public byte Labels { get; set; }

        /// <summary>
        ///     Gets or sets the original TTL
        /// </summary>
        public UInt32 OriginalTTL { get; set; }

        /// <summary>
        ///     Gets or sets the signature expiration
        /// </summary>
        public UInt32 SignatureExpiration { get; set; }

        /// <summary>
        ///     Gets or sets the signature inception
        /// </summary>
        public UInt32 SignatureInception { get; set; }

        /// <summary>
        ///     Gets or sets the key tag
        /// </summary>
        public UInt16 KeyTag { get; set; }

        /// <summary>
        ///     Gets or sets the signers name
        /// </summary>
        public string SignersName { get; set; }

        /// <summary>
        ///     Gets or sets the signature
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSIG" /> class
        /// </summary>
        /// <param name="rr">Record reader for the record data</param>
        public RecordSIG(RecordReader rr)
        {
            this.TypeCovered = rr.ReadUInt16();
            this.Algorithm = rr.ReadByte();
            this.Labels = rr.ReadByte();
            this.OriginalTTL = rr.ReadUInt32();
            this.SignatureExpiration = rr.ReadUInt32();
            this.SignatureInception = rr.ReadUInt32();
            this.KeyTag = rr.ReadUInt16();
            this.SignersName = rr.ReadDomainName();
            this.Signature = rr.ReadString();
        }

        /// <summary>
        ///     Get a string version of the record
        /// </summary>
        /// <returns>String of the record</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4} {5} {6} {7} \"{8}\"",
                this.TypeCovered,
                this.Algorithm,
                this.Labels,
                this.OriginalTTL,
                this.SignatureExpiration,
                this.SignatureInception,
                this.KeyTag,
                this.SignersName,
                this.Signature);
        }

    }
}