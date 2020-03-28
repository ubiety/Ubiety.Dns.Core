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
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS Record header.
    /// </summary>
    public class Header
    {
        // internal flag
        private ushort _flags;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Header" /> class.
        /// </summary>
        public Header()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Header" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> of the record.</param>
        internal Header(RecordReader reader)
        {
            Id = reader.ReadUInt16();
            _flags = reader.ReadUInt16();
            QuestionCount = reader.ReadUInt16();
            AnswerCount = reader.ReadUInt16();
            NameserverCount = reader.ReadUInt16();
            AdditionalRecordsCount = reader.ReadUInt16();
        }

        /// <summary>
        ///     Gets or sets the unique identifier of the record.
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        ///     Gets or sets the number of questions in the record.
        /// </summary>
        public ushort QuestionCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of answers in the record.
        /// </summary>
        public ushort AnswerCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of name servers in the record.
        /// </summary>
        public ushort NameserverCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of additional records in the record.
        /// </summary>
        public ushort AdditionalRecordsCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the record is a query or response.
        /// </summary>
        public bool QueryResponse
        {
            get => GetBits(_flags, 15, 1) == 1;
            set => _flags = SetBits(_flags, 15, 1, value);
        }

        /// <summary>
        ///     Gets or sets the record operation code flag.
        /// </summary>
        public OperationCode OpCode
        {
            get => (OperationCode)GetBits(_flags, 11, 4);
            set => _flags = SetBits(_flags, 11, 4, (ushort)value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the record is an authoritative answer.
        /// </summary>
        public bool AuthoritativeAnswer
        {
            get => GetBits(_flags, 10, 1) == 1;
            set => _flags = SetBits(_flags, 10, 1, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the truncation flag is set.
        /// </summary>
        public bool Truncation
        {
            get => GetBits(_flags, 9, 1) == 1;
            set => _flags = SetBits(_flags, 9, 1, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion flag is set.
        /// </summary>
        public bool Recursion
        {
            get => GetBits(_flags, 8, 1) == 1;
            set => _flags = SetBits(_flags, 8, 1, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion available flag is set.
        /// </summary>
        public bool RecursionAvailable
        {
            get => GetBits(_flags, 7, 1) == 1;
            set => _flags = SetBits(_flags, 7, 1, value);
        }

        /// <summary>
        ///     Gets or sets a record reserved flag.
        /// </summary>
        public ushort Z
        {
            get => GetBits(_flags, 4, 3);
            set => _flags = SetBits(_flags, 4, 3, value);
        }

        /// <summary>
        ///     Gets or sets the record response code.
        /// </summary>
        public ResponseCode ResponseCode
        {
            get => (ResponseCode)GetBits(_flags, 0, 4);
            set => _flags = SetBits(_flags, 0, 4, (ushort)value);
        }

        /// <summary>
        ///     Gets the header as a byte array.
        /// </summary>
        /// <returns>Byte array of the header data.</returns>
        public IEnumerable<byte> GetData()
        {
            var data = new List<byte>();
            data.AddRange(Id.GetBytes());
            data.AddRange(_flags.GetBytes());
            data.AddRange(QuestionCount.GetBytes());
            data.AddRange(AnswerCount.GetBytes());
            data.AddRange(NameserverCount.GetBytes());
            data.AddRange(AdditionalRecordsCount.GetBytes());
            return data.ToArray();
        }

        private static ushort SetBits(ushort oldValue, int position, int length, bool blnValue)
        {
            return SetBits(oldValue, position, length, blnValue ? (ushort)1 : (ushort)0);
        }

        private static ushort SetBits(ushort oldValue, int position, int length, ushort newValue)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return oldValue;
            }

            // get some mask to put on
            var mask = (2 << (length - 1)) - 1;

            // clear out value
            oldValue &= (ushort)~(mask << position);

            // set new value
            oldValue |= (ushort)((newValue & mask) << position);
            return oldValue;
        }

        private static ushort GetBits(ushort oldValue, int position, int length)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return 0;
            }

            // get some mask to put on
            var mask = (2 << (length - 1)) - 1;

            // shift down to get some value and mask it
            return (ushort)((oldValue >> position) & mask);
        }
    }
}