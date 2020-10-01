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
    public record RecordRp : Record
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
