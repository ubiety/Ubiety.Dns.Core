using System;
/*
 * http://www.ietf.org/rfc/rfc2845.txt
 * 
 * Field Name       Data Type      Notes
      --------------------------------------------------------------
      Algorithm Name   domain-name    Name of the algorithm
                                      in domain name syntax.
      Time Signed      u_int48_t      seconds since 1-Jan-70 UTC.
      Fudge            u_int16_t      seconds of error permitted
                                      in Time Signed.
      MAC Size         u_int16_t      number of octets in MAC.
      MAC              octet stream   defined by Algorithm Name.
      Original ID      u_int16_t      original message ID
      Error            u_int16_t      expanded RCODE covering
                                      TSIG processing.
      Other Len        u_int16_t      length, in octets, of
                                      Other Data.
      Other Data       octet stream   empty unless Error == BADTIME

 */

namespace Heijden.DNS
{
        /// <summary>
        /// </summary>
    public class RecordTSIG : Record
    {
        /// <summary>
        /// </summary>
        public string ALGORITHMNAME;
        /// <summary>
        /// </summary>
        public long TIMESIGNED;
        /// <summary>
        /// </summary>
        public UInt16 FUDGE;
        /// <summary>
        /// </summary>
        public UInt16 MACSIZE;
        /// <summary>
        /// </summary>
        public byte[] MAC;
        /// <summary>
        /// </summary>
        public UInt16 ORIGINALID;
        /// <summary>
        /// </summary>
        public UInt16 ERROR;
        /// <summary>
        /// </summary>
        public UInt16 OTHERLEN;
        /// <summary>
        /// </summary>
        public byte[] OTHERDATA;

        /// <summary>
        /// </summary>
        public RecordTSIG(RecordReader rr)
        {
            ALGORITHMNAME = rr.ReadDomainName();
            TIMESIGNED = rr.ReadUInt32() << 32 | rr.ReadUInt32();
            FUDGE = rr.ReadUInt16();
            MACSIZE = rr.ReadUInt16();
            MAC = rr.ReadBytes(MACSIZE);
            ORIGINALID = rr.ReadUInt16();
            ERROR = rr.ReadUInt16();
            OTHERLEN = rr.ReadUInt16();
            OTHERDATA = rr.ReadBytes(OTHERLEN);
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(TIMESIGNED);
            string printDate = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            return string.Format("{0} {1} {2} {3} {4}",
                ALGORITHMNAME,
                printDate,
                FUDGE,
                ORIGINALID,
                ERROR);
        }

    }
}
