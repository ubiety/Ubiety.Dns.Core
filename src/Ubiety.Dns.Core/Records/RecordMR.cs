using System;
/*
3.3.8. MR RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   NEWNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

NEWNAME         A <domain-name> which specifies a mailbox which is the
                proper rename of the specified mailbox.

MR records cause no additional section processing.  The main use for MR
is as a forwarding entry for a user who has moved to a different
mailbox.
*/
namespace Ubiety.Dns.Core.Records
{
        /// <summary>
        /// </summary>
    public class RecordMR : Record
    {
        /// <summary>
        /// </summary>
        public string NEWNAME;

        /// <summary>
        /// </summary>
        public RecordMR(RecordReader rr)
        {
            NEWNAME = rr.ReadDomainName();
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return NEWNAME;
        }

    }
}
