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

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mailbox rename DNS record.
    /// </summary>
    public record RecordMr : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMr" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordMr(RecordReader reader)
            : base(reader)
        {
            NewName = Reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets the new name.
        /// </summary>
        public string NewName { get; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Rename domain from the record.</returns>
        public override string ToString()
        {
            return NewName;
        }
    }
}
