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

/* http://tools.ietf.org/rfc/rfc1183.txt

2.2. The Responsible Person RR

   The method uses a new RR type with mnemonic RP and type code of 17
   (decimal).

   RP has the following format:

   <owner> <ttl> <class> RP <mbox-dname> <txt-dname>

   Both RDATA fields are required in all RP RRs.

   The first field, <mbox-dname>, is a domain name that specifies the
   mailbox for the responsible person.  Its format in master files uses
   the DNS convention for mailbox encoding, identical to that used for
   the RNAME mailbox field in the SOA RR.  The root domain name (just
   ".") may be specified for <mbox-dname> to indicate that no mailbox is
   available.

   The second field, <txt-dname>, is a domain name for which TXT RR's
   exist.  A subsequent query can be performed to retrieve the
   associated TXT resource records at <txt-dname>.  This provides a
   level of indirection so that the entity can be referred to from
   multiple places in the DNS.  The root domain name (just ".") may be
   specified for <txt-dname> to indicate that the TXT_DNAME is absent,
   and no associated TXT RR exists.

 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Responsible person DNS record.
    /// </summary>
    public class RecordRp : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordRp" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordRp(RecordReader reader)
            : base(reader)
        {
            MailboxDomain = Reader.ReadDomainName();
            TxtDomain = Reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the mailbox domain.
        /// </summary>
        public string MailboxDomain { get; set; }

        /// <summary>
        ///     Gets or sets the text domain.
        /// </summary>
        public string TxtDomain { get; set; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Domains as a string.</returns>
        public override string ToString()
        {
            return $"{MailboxDomain} {TxtDomain}";
        }
    }
}
