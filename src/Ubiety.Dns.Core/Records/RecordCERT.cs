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
    ///     Certificate DNS record
    /// </summary>
    public class RecordCert : Record
    {
        private readonly Byte[] _rawKey;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordCert" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordCert(RecordReader rr)
        {
            // re-read length
            var recordLength = rr.ReadUInt16(-2);

            Type = rr.ReadUInt16();
            KeyTag = rr.ReadUInt16();
            Algorithm = rr.ReadByte();
            var length = recordLength - 5;
            _rawKey = rr.ReadBytes(length);
            PublicKey = Convert.ToBase64String(_rawKey);
        }

        /// <summary>
        ///     Gets the record type
        /// </summary>
        public UInt16 Type { get; }

        /// <summary>
        ///     Gets the key tag
        /// </summary>
        public UInt16 KeyTag { get; }

        /// <summary>
        ///     Gets the algorithm
        /// </summary>
        public Byte Algorithm { get; }

        /// <summary>
        ///     Gets the public key
        /// </summary>
        public String PublicKey { get; }

        /// <summary>
        ///     Gets the raw key
        /// </summary>
        public List<Byte> RawKey => new List<Byte>(_rawKey);

        /// <summary>
        ///     String version of the record
        /// </summary>
        /// <returns>String of the public key</returns>
        public override String ToString()
        {
            return PublicKey;
        }
    }
}