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
    public class RecordTxt : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordTxt" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        /// <param name="length">Record length.</param>
        public RecordTxt(RecordReader reader, int length)
            : base(reader)
        {
            var position = Reader.Position;
            Text = new List<string>();
            while ((Reader.Position - position) < length)
            {
                Text.Add(Reader.ReadString());
            }
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
