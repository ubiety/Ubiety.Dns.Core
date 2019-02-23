/*
 3.3.7. MINFO RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                    RMAILBX                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                    EMAILBX                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

RMAILBX         A <domain-name> which specifies a mailbox which is
                responsible for the mailing list or mailbox.  If this
                domain name names the root, the owner of the MINFO RR is
                responsible for itself.  Note that many existing mailing
                lists use a mailbox X-request for the RMAILBX field of
                mailing list X, e.g., Msgroup-request for Msgroup.  This
                field provides a more general mechanism.


EMAILBX         A <domain-name> which specifies a mailbox which is to
                receive error messages related to the mailing list or
                mailbox specified by the owner of the MINFO RR (similar
                to the ERRORS-TO: field which has been proposed).  If
                this domain name names the root, errors should be
                returned to the sender of the message.

MINFO records cause no additional section processing.  Although these
records can be associated with a simple mailbox, they are usually used
with a mailing list.
 */

using System;

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mail list DNS record
    /// </summary>
    public class RecordMinfo : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMinfo" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordMinfo(RecordReader rr)
        {
            ResponsibleMailbox = rr.ReadDomainName();
            ErrorMailbox = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets the responsible mailbox
        /// </summary>
        public string ResponsibleMailbox { get; }

        /// <summary>
        ///     Gets the error mailbox
        /// </summary>
        public string ErrorMailbox { get; }

        /// <summary>
        ///     String representation of the record
        /// </summary>
        /// <returns>String version of the domains</returns>
        public override string ToString()
        {
            return $"{ResponsibleMailbox} {ErrorMailbox}";
        }
    }
}