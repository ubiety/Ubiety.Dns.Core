using System;
/*
 * http://tools.ietf.org/rfc/rfc2930.txt
 * 
2. The TKEY Resource Record

   The TKEY resource record (RR) has the structure given below.  Its RR
   type code is 249.

      Field       Type         Comment
      -----       ----         -------
       Algorithm:   domain
       Inception:   u_int32_t
       Expiration:  u_int32_t
       Mode:        u_int16_t
       Error:       u_int16_t
       Key Size:    u_int16_t
       Key Data:    octet-stream
       Other Size:  u_int16_t
       Other Data:  octet-stream  undefined by this specification

 */

namespace Ubiety.Dns.Core.Records
{
        /// <summary>
        /// </summary>
    public class RecordTKEY : Record
    {
        /// <summary>
        /// </summary>
        public string ALGORITHM;
        /// <summary>
        /// </summary>
        public UInt32 INCEPTION;
        /// <summary>
        /// </summary>
        public UInt32 EXPIRATION;
        /// <summary>
        /// </summary>
        public UInt16 MODE;
        /// <summary>
        /// </summary>
        public UInt16 ERROR;
        /// <summary>
        /// </summary>
        public UInt16 KEYSIZE;
        /// <summary>
        /// </summary>
        public byte[] KEYDATA;
        /// <summary>
        /// </summary>
        public UInt16 OTHERSIZE;
        /// <summary>
        /// </summary>
        public byte[] OTHERDATA;

        /// <summary>
        /// </summary>
        public RecordTKEY(RecordReader rr)
        {
            ALGORITHM = rr.ReadDomainName();
            INCEPTION = rr.ReadUInt32();
            EXPIRATION = rr.ReadUInt32();
            MODE = rr.ReadUInt16();
            ERROR = rr.ReadUInt16();
            KEYSIZE = rr.ReadUInt16();
            KEYDATA = rr.ReadBytes(KEYSIZE);
            OTHERSIZE = rr.ReadUInt16();
            OTHERDATA = rr.ReadBytes(OTHERSIZE);
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                ALGORITHM,
                INCEPTION,
                EXPIRATION,
                MODE,
                ERROR);
        }

    }
}
