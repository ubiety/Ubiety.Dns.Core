using System;
/*
3.3.3. MB RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has the
                specified mailbox.

MB records cause additional section processing which looks up an A type
RRs corresponding to MADNAME.
*/
namespace Heijden.DNS
{
        /// <summary>
        /// </summary>
    public class RecordMB : Record
    {
        /// <summary>
        /// </summary>
        public string MADNAME;

        /// <summary>
        /// </summary>
        public RecordMB(RecordReader rr)
        {
            MADNAME = rr.ReadDomainName();
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return MADNAME;
        }

    }
}
