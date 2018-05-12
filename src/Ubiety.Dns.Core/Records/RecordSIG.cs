using System;

#region Rfc info
/*
 * http://www.ietf.org/rfc/rfc2535.txt
 * 4.1 SIG RDATA Format

   The RDATA portion of a SIG RR is as shown below.  The integrity of
   the RDATA information is protected by the signature field.

                           1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
       0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |        type covered           |  algorithm    |     labels    |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                         original TTL                          |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature expiration                     |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |                      signature inception                      |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      |            key  tag           |                               |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+         signer's name         +
      |                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-/
      /                                                               /
      /                            signature                          /
      /                                                               /
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+


*/
#endregion

namespace Heijden.DNS
{
        /// <summary>
        /// </summary>
    public class RecordSIG : Record
    {
        /// <summary>
        /// </summary>
        public UInt16 TYPECOVERED;
        /// <summary>
        /// </summary>
        public byte ALGORITHM;
        /// <summary>
        /// </summary>
        public byte LABELS;
        /// <summary>
        /// </summary>
        public UInt32 ORIGINALTTL;
        /// <summary>
        /// </summary>
        public UInt32 SIGNATUREEXPIRATION;
        /// <summary>
        /// </summary>
        public UInt32 SIGNATUREINCEPTION;
        /// <summary>
        /// </summary>
        public UInt16 KEYTAG;
        /// <summary>
        /// </summary>
        public string SIGNERSNAME;
        /// <summary>
        /// </summary>
        public string SIGNATURE;

        /// <summary>
        /// </summary>
        public RecordSIG(RecordReader rr)
        {
            TYPECOVERED = rr.ReadUInt16();
            ALGORITHM = rr.ReadByte();
            LABELS = rr.ReadByte();
            ORIGINALTTL = rr.ReadUInt32();
            SIGNATUREEXPIRATION = rr.ReadUInt32();
            SIGNATUREINCEPTION = rr.ReadUInt32();
            KEYTAG = rr.ReadUInt16();
            SIGNERSNAME = rr.ReadDomainName();
            SIGNATURE = rr.ReadString();
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} \"{8}\"",
                TYPECOVERED,
                ALGORITHM,
                LABELS,
                ORIGINALTTL,
                SIGNATUREEXPIRATION,
                SIGNATUREINCEPTION,
                KEYTAG,
                SIGNERSNAME,
                SIGNATURE);
        }

    }
}
