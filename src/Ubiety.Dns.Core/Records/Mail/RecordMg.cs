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

using System;

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mail group DNS record.
    /// </summary>
    public class RecordMg : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMg" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordMg(RecordReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            MgmName = reader.ReadDomainName();
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