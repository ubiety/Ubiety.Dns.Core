using System;
using System.Collections.Generic;
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
        private readonly byte[] _digest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordDs" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record data</param>
        public RecordDs(RecordReader rr)
        {
            var length = rr.ReadUInt16(-2);
            KeyTag = rr.ReadUInt16();
            Algorithm = rr.ReadByte();
            DigestType = rr.ReadByte();
            length -= 4;
            _digest = new byte[length];
            _digest = rr.ReadBytes(length);
        }

        /// <summary>
        ///     Gets the key tag
        /// </summary>
        public ushort KeyTag { get; }

        /// <summary>
        ///     Gets the algorithm
        /// </summary>
        public byte Algorithm { get; }

        /// <summary>
        ///     Gets the digest type
        /// </summary>
        public byte DigestType { get; }

        /// <summary>
        ///     Gets the digest
        /// </summary>
        public List<byte> Digest => new List<byte>(_digest);

        /// <summary>
        ///     String version of the record
        /// </summary>
        /// <returns>String of the data</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var t in _digest)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", t);
            }

            return $"{KeyTag} {Algorithm} {DigestType} {sb}";
        }
    }
}