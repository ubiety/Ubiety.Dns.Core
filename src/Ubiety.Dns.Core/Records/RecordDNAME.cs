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
 * http://tools.ietf.org/rfc/rfc2672.txt
 *
3. The DNAME Resource Record

   The DNAME RR has mnemonic DNAME and type code 39 (decimal).
   DNAME has the following format:

      <owner> <ttl> <class> DNAME <target>

   The format is not class-sensitive.  All fields are required.  The
   RDATA field <target> is a <domain-name> [DNSIS].

 *
 */

using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNAME DNS Record.
    /// </summary>
    public class RecordDname : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordDname" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record.</param>
        public RecordDname(RecordReader rr)
        {
            Target = rr.ThrowIfNull(nameof(rr)).ReadDomainName();
        }

        /// <summary>
        ///     Gets the target.
        /// </summary>
        public string Target { get; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>String of the target.</returns>
        public override string ToString()
        {
            return Target;
        }
    }
}