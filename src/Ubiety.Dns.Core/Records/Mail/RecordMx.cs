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

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mail exchange DNS record.
    /// </summary>
    /// <remarks>
    ///     # [Description](#tab/description)
    ///     Standard MX mail DNS record
    ///     # [RFC](#tab/rfc)
    ///     ```
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     |                  PREFERENCE                   |
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     /                   EXCHANGE                    /
    ///     /                                               /
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     where:
    ///     PREFERENCE      A 16 bit integer which specifies the preference given to
    ///     this RR among others at the same owner.  Lower values
    ///     are preferred.
    ///     EXCHANGE        A [domain-name] which specifies a host willing to act as
    ///     a mail exchange for the owner name.
    ///     MX records cause type A additional section processing for the host
    ///     specified by EXCHANGE.  The use of MX RRs is explained in detail in
    ///     [RFC-974].
    ///     ```.
    /// </remarks>
    public sealed record RecordMx : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMx" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordMx(RecordReader reader)
            : base(reader)
        {
            Preference = Reader.ReadUInt16();
            Exchange = Reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets the preference.
        /// </summary>
        public ushort Preference { get; }

        /// <summary>
        ///     Gets the exchange.
        /// </summary>
        public string Exchange { get; }
    }
}
