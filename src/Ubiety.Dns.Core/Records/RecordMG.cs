using System;
/*
3.3.6. MG RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MGMNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MGMNAME         A <domain-name> which specifies a mailbox which is a
                member of the mail group specified by the domain name.

MG records cause no additional section processing.
*/
namespace Ubiety.Dns.Core.Records
{
        /// <summary>
        /// </summary>
    public class RecordMG : Record
    {
        /// <summary>
        /// </summary>
        public string MGMNAME;

        /// <summary>
        /// </summary>
        public RecordMG(RecordReader rr)
        {
            MGMNAME = rr.ReadDomainName();
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return MGMNAME;
        }

    }
}
