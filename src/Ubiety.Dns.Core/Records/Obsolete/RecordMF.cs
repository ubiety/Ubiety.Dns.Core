/*
 *
3.3.5. MF RDATA format (Obsolete)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has a mail
                agent for the domain which will accept mail for
                forwarding to the domain.

MF records cause additional section processing which looks up an A type
record corresponding to MADNAME.

MF is obsolete.  See the definition of MX and [RFC-974] for details ofw
the new scheme.  The recommended policy for dealing with MD RRs found in
a master file is to reject them, or to convert them to MX RRs with a
preference of 10. */

using System;

namespace Ubiety.Dns.Core.Records.Obsolete
{
    /// <summary>
    ///     Mail forwarder record (Obsolete - use MX).
    /// </summary>
    public class RecordMf : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMf" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordMf(RecordReader rr)
        {
            MadName = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets the mail domain.
        /// </summary>
        public string MadName { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String of the mail domain.</returns>
        public override string ToString()
        {
            return MadName;
        }
    }
}