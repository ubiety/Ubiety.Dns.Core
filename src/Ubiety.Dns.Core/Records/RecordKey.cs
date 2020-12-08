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

/* http://www.ietf.org/rfc/rfc2535.txt
 *
3.1 KEY RDATA format

   The RDATA for a KEY RR consists of flags, a protocol octet, the
   algorithm number octet, and the public key itself.  The format is as
   follows:
                        1 1 1 1 1 1 1 1 1 1 2 2 2 2 2 2 2 2 2 2 3 3
    0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |             flags             |    protocol   |   algorithm   |
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   |                                                               /
   /                          public key                           /
   /                                                               /
   +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-|

   The KEY RR is not intended for storage of certificates and a separate
   certificate RR has been developed for that purpose, defined in [RFC
   2538].

   The meaning of the KEY RR owner name, flags, and protocol octet are
   described in Sections 3.1.1 through 3.1.5 below.  The flags and
   algorithm must be examined before any data following the algorithm
   octet as they control the existence and format of any following data.
   The algorithm and public key fields are described in Section 3.2.
   The format of the public key is algorithm dependent.

   KEY RRs do not specify their validity period but their authenticating
   SIG RR(s) do as described in Section 4 below.

*/

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Key DNS record.
    /// </summary>
    public record RecordKey : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordKey" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordKey(RecordReader reader)
            : base(reader)
        {
            Flags = Reader.ReadUInt16();
            Protocol = Reader.ReadByte();
            Algorithm = Reader.ReadByte();
            PublicKey = Reader.ReadString();
        }

        /// <summary>
        ///     Gets or sets the flags.
        /// </summary>
        public ushort Flags { get; set; }

        /// <summary>
        ///     Gets or sets the protocol.
        /// </summary>
        public byte Protocol { get; set; }

        /// <summary>
        ///     Gets or sets the algorithm.
        /// </summary>
        public byte Algorithm { get; set; }

        /// <summary>
        ///     Gets or sets the public key.
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>String version of the record.</returns>
        public override string ToString()
        {
            return $"{Flags} {Protocol} {Algorithm} \"{PublicKey}\"";
        }
    }
}
