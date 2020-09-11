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
 3.3.2. HINFO RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                      CPU                      /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                       OS                      /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

CPU             A <character-string> which specifies the CPU type.

OS              A <character-string> which specifies the operating
                system type.

Standard values for CPU and OS can be found in [RFC-1010].

HINFO records are used to acquire general information about a host.  The
main use is for protocols such as FTP that can use special procedures
when talking between machines or operating systems of the same type.
 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Hardware info DNS record.
    /// </summary>
    public record RecordHinfo : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordHinfo" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordHinfo(RecordReader reader)
            : base(reader)
        {
            Cpu = Reader.ReadString();
            Os = Reader.ReadString();
        }

        /// <summary>
        ///     Gets the CPU.
        /// </summary>
        public string Cpu { get; }

        /// <summary>
        ///     Gets the OS.
        /// </summary>
        public string Os { get; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>String version of the record.</returns>
        public override string ToString()
        {
            return $"CPU={Cpu} OS={Os}";
        }
    }
}
