/*
3.3.4. MD RDATA format (Obsolete)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has a mail
                agent for the domain which should be able to deliver
                mail for the domain.

MD records cause additional section processing which looks up an A type
record corresponding to MADNAME.

MD is obsolete.  See the definition of MX and [RFC-974] for details of
the new scheme.  The recommended policy for dealing with MD RRs found in
a master file is to reject them, or to convert them to MX RRs with a
preference of 0.
 * */

using System;

namespace Ubiety.Dns.Core.Records.Obsolete
{
    /// <summary>
    ///     Mail domain record
    /// </summary>
    public class RecordMd : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMd" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordMd(RecordReader rr)
        {
            MadName = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets the mail domain
        /// </summary>
        public string MadName { get; }

        /// <summary>
        ///     String representing the mail domain
        /// </summary>
        /// <returns>String version of the record</returns>
        public override string ToString()
        {
            return MadName;
        }
    }
}