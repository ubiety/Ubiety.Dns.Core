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
3.3.14. TXT RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   TXT-DATA                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

TXT-DATA        One or more <character-string>s.

TXT RRs are used to hold descriptive text.  The semantics of the text
depends on the domain where it is found.
 *
*/

using System.Collections.Generic;
using System.Text;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Text DNS record.
    /// </summary>
    public record RecordTxt : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordTxt" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordTxt(RecordReader reader)
            : base(reader)
        {
            Text = new List<string>
            {
                Reader.ReadString()
            };
        }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        public List<string> Text { get; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Text as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in Text)
            {
                sb.Append(item);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
