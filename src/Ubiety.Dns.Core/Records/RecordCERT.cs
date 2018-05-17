using System;
using System.Collections.ObjectModel;

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
    ///     Certificate DNS record
    /// </summary>
    public class RecordCert : Record
    {
        private Byte[] rawKey;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordCert" /> class
        /// </summary>
        public RecordCert(RecordReader rr)
        {
            // re-read length
            UInt16 recordLength = rr.ReadUInt16(-2);

            this.Type = rr.ReadUInt16();
            this.KeyTag = rr.ReadUInt16();
            this.Algorithm = rr.ReadByte();
            var length = recordLength - 5;
            this.rawKey = rr.ReadBytes(length);
            this.PublicKey = Convert.ToBase64String(this.rawKey);
        }

        /// <summary>
        ///     Gets or sets the record type
        /// </summary>
        public UInt16 Type { get; set; }

        /// <summary>
        ///     Gets or sets the key tag
        /// </summary>
        public UInt16 KeyTag { get; set; }

        /// <summary>
        ///     Gets or sets the algorithm
        /// </summary>
        public Byte Algorithm { get; set; }

        /// <summary>
        ///     Gets or sets the public key
        /// </summary>
        public String PublicKey { get; set; }

        /// <summary>
        ///     Gets the raw key
        /// </summary>
        public Collection<Byte> RawKey { get => new Collection<Byte>(this.rawKey); }

        /// <summary>
        ///     String version of the record
        /// </summary>
        /// <returns>String of the public key</returns>
        public override String ToString()
        {
            return this.PublicKey;
        }
    }
}
