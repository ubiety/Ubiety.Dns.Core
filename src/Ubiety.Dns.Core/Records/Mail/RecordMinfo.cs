/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
