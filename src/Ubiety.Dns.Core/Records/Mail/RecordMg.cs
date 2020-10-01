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

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mail group DNS record.
    /// </summary>
    public record RecordMg : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMg" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordMg(RecordReader reader)
            : base(reader)
        {
            MgmName = Reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets the mail group name.
        /// </summary>
        public string MgmName { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>Mail group name as a string.</returns>
        public override string ToString()
        {
            return MgmName;
        }
    }
}
