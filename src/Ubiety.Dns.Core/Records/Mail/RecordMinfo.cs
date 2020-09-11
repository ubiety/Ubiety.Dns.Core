/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mail list DNS record.
    /// </summary>
    public record RecordMinfo : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMinfo" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordMinfo(RecordReader reader)
            : base(reader)
        {
            ResponsibleMailbox = Reader.ReadDomainName();
            ErrorMailbox = Reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets the responsible mailbox.
        /// </summary>
        public string ResponsibleMailbox { get; }

        /// <summary>
        ///     Gets the error mailbox.
        /// </summary>
        public string ErrorMailbox { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String version of the domains.</returns>
        public override string ToString()
        {
            return $"{ResponsibleMailbox} {ErrorMailbox}";
        }
    }
}
