/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
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