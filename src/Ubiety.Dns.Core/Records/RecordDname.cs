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

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNAME DNS Record.
    /// </summary>
    public record RecordDname : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordDname" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record.</param>
        public RecordDname(RecordReader reader)
            : base(reader)
        {
            Target = Reader.ReadDomainName();
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
