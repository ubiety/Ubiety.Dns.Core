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
3.3.3. MB RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has the
                specified mailbox.

MB records cause additional section processing which looks up an A type
RRs corresponding to MADNAME.
*/

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mailbox DNS record.
    /// </summary>
    public record RecordMb : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMb" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordMb(RecordReader reader)
            : base(reader)
        {
            MadName = Reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets the mailbox domain.
        /// </summary>
        public string MadName { get; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>String version of the domain.</returns>
        public override string ToString()
        {
            return MadName;
        }
    }
}
