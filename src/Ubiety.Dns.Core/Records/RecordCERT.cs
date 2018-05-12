using System;
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
        /// </summary>
    public class RecordCERT : Record
    {
        /// <summary>
        /// </summary>
        public byte[] RDATA;
        /// <summary>
        /// </summary>
        public ushort TYPE;
        /// <summary>
        /// </summary>
        public ushort KEYTAG;  //Format
        /// <summary>
        /// </summary>
        public byte ALGORITHM;
        /// <summary>
        /// </summary>
        public string PUBLICKEY;
        /// <summary>
        /// </summary>
        public byte[] RAWKEY;

        /// <summary>
        /// </summary>
        public RecordCERT(RecordReader rr)
        {
            // re-read length
            ushort RDLENGTH = rr.ReadUInt16(-2);
            //RDATA = rr.ReadBytes(RDLENGTH);

            TYPE = rr.ReadUInt16();
            KEYTAG = rr.ReadUInt16();
            ALGORITHM = rr.ReadByte();
            var length = RDLENGTH - 5;
            RAWKEY = rr.ReadBytes(length);
            PUBLICKEY = Convert.ToBase64String(RAWKEY);
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return PUBLICKEY;
        }

    }
}
