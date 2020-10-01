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
 * http://www.faqs.org/rfcs/rfc2915.html
 *
 8. DNS Packet Format

         The packet format for the NAPTR record is:

                                          1  1  1  1  1  1
            0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
          +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          |                     ORDER                     |
          +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          |                   PREFERENCE                  |
          +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          /                     FLAGS                     /
          +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          /                   SERVICES                    /
          +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          /                    REGEXP                     /
          +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
          /                  REPLACEMENT                  /
          /                                               /
          +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

    where:

   FLAGS A <character-string> which contains various flags.

   SERVICES A <character-string> which contains protocol and service
      identifiers.

   REGEXP A <character-string> which contains a regular expression.

   REPLACEMENT A <domain-name> which specifies the new value in the
      case where the regular expression is a simple replacement
      operation.

   <character-string> and <domain-name> as used here are defined in
   RFC1035 [1].

 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     NAPTR DNS record.
    /// </summary>
    public record RecordNaptr : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordNaptr" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordNaptr(RecordReader reader)
            : base(reader)
        {
            Order = Reader.ReadUInt16();
            Preference = Reader.ReadUInt16();
            Flags = Reader.ReadString();
            Services = Reader.ReadString();
            Regexp = Reader.ReadString();
            Replacement = Reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        public ushort Order { get; set; }

        /// <summary>
        ///     Gets or sets the preference.
        /// </summary>
        public ushort Preference { get; set; }

        /// <summary>
        ///     Gets or sets the flags.
        /// </summary>
        public string Flags { get; set; }

        /// <summary>
        ///     Gets or sets the services.
        /// </summary>
        public string Services { get; set; }

        /// <summary>
        ///     Gets or sets the regexp.
        /// </summary>
        public string Regexp { get; set; }

        /// <summary>
        ///     Gets or sets the replacement.
        /// </summary>
        public string Replacement { get; set; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Data as a string.</returns>
        public override string ToString()
        {
            return $"{Order} {Preference} \"{Flags}\" \"{Services}\" \"{Regexp}\" {Replacement}";
        }
    }
}
