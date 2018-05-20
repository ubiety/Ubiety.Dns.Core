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
    ///     Mail group DNS record
    /// </summary>
    public class RecordMg : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMg" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordMg(RecordReader rr)
        {
            this.MgmName = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the mail group name
        /// </summary>
        public String MgmName { get; set; }

        /// <summary>
        ///     String representation of the record
        /// </summary>
        /// <returns>Mail group name as a string</returns>
        public override String ToString()
        {
            return this.MgmName;
        }
    }
}
