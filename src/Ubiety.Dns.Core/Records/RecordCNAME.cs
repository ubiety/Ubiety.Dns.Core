using System;
/*
 * 
3.3.1. CNAME RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     CNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

CNAME           A <domain-name> which specifies the canonical or primary
                name for the owner.  The owner name is an alias.

CNAME RRs cause no additional section processing, but name servers may
choose to restart the query at the canonical name in certain cases.  See
the description of name server logic in [RFC-1034] for details.

 * 
 */
namespace Heijden.DNS
{
        /// <summary>
        /// </summary>
    public class RecordCNAME : Record
    {
        /// <summary>
        /// </summary>
        public string CNAME;

        /// <summary>
        /// </summary>
        public RecordCNAME(RecordReader rr)
        {
            CNAME = rr.ReadDomainName();
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return CNAME;
        }

    }
}
