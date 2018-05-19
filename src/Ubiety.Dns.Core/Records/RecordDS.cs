using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

/*
 * http://tools.ietf.org/rfc/rfc3658.txt
 *
2.4.  Wire Format of the DS record

   The DS (type=43) record contains these fields: key tag, algorithm,
   digest type, and the digest of a public key KEY record that is
   allowed and/or used to sign the child's apex KEY RRset.  Other keys
   MAY sign the child's apex KEY RRset.

                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |           key tag             |  algorithm    |  Digest type  |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                digest  (length depends on type)               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                (SHA-1 digest is 20 bytes)                     |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|
   |                                                               |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DS DNS Record
    /// </summary>
    public class RecordDs : Record
    {
        private Byte[] digest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordDs" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record data</param>
        public RecordDs(RecordReader rr)
        {
            UInt16 length = rr.ReadUInt16(-2);
            this.KeyTag = rr.ReadUInt16();
            this.Algorithm = rr.ReadByte();
            this.DigestType = rr.ReadByte();
            length -= 4;
            this.digest = new Byte[length];
            this.digest = rr.ReadBytes(length);
        }

        /// <summary>
        ///     Gets or sets the key tag
        /// </summary>
        public UInt16 KeyTag { get; set; }

        /// <summary>
        ///     Gets or sets the algorithm
        /// </summary>
        public Byte Algorithm { get; set; }

        /// <summary>
        ///     Gets or sets the digest type
        /// </summary>
        public Byte DigestType { get; set; }

        /// <summary>
        ///     Gets the digest
        /// </summary>
        public Collection<Byte> Digest { get => new Collection<Byte>(this.digest); }

        /// <summary>
        ///     String version of the record
        /// </summary>
        /// <returns>String of the data</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (Int32 i = 0; i < this.digest.Length; i++)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", this.digest[i]);
            }

            return $"{this.KeyTag} {this.Algorithm} {this.DigestType} {sb.ToString()}";
        }
    }
}
