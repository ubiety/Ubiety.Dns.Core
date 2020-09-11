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
