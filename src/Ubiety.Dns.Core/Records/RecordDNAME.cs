/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
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

using System;
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