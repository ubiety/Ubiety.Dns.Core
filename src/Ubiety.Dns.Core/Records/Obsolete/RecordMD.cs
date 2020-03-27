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
3.3.4. MD RDATA format (Obsolete)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has a mail
                agent for the domain which should be able to deliver
                mail for the domain.

MD records cause additional section processing which looks up an A type
record corresponding to MADNAME.

MD is obsolete.  See the definition of MX and [RFC-974] for details of
the new scheme.  The recommended policy for dealing with MD RRs found in
a master file is to reject them, or to convert them to MX RRs with a
preference of 0.
 * */

using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core.Records.Obsolete
{
    /// <summary>
    ///     Mail domain record.
    /// </summary>
    public class RecordMd : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMd" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordMd(RecordReader rr)
        {
            MadName = rr.ThrowIfNull(nameof(rr)).ReadDomainName();
        }

        /// <summary>
        ///     Gets the mail domain.
        /// </summary>
        public string MadName { get; }

        /// <summary>
        ///     String representing the mail domain.
        /// </summary>
        /// <returns>String version of the record.</returns>
        public override string ToString()
        {
            return MadName;
        }
    }
}