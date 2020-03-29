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

using System.Collections.Generic;
using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Abstract record.
    /// </summary>
    public abstract class Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Record" /> class.
        /// </summary>
        protected Record()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Record" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        protected Record(RecordReader reader)
        {
            Reader = reader.ThrowIfNull(nameof(reader));
            var length = Reader.ReadUInt16(-2);
            RecordData = new List<byte>(Reader.ReadBytes(length));
        }

        /// <summary>
        ///     Gets the record data.
        /// </summary>
        /// <value>Byte list of the raw record data.</value>
        public List<byte> RecordData { get; }

        /// <summary>
        ///     Gets or sets the resource record this record is a part of.
        /// </summary>
        /// <value>Resource record of the data.</value>
        public ResourceRecord ResourceRecord { get; set; }

        /// <summary>
        ///     Gets the record reader for the record.
        /// </summary>
        protected RecordReader Reader { get; }

        /// <summary>
        ///     String representation of the record.
        /// </summary>
        /// <returns>String version of the data.</returns>
        public override string ToString()
        {
            return $"{GetType().Name} is not-used";
        }
    }
}