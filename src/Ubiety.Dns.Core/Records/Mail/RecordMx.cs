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
