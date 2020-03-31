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
        ///     Gets or sets a value indicating whether the record is a query or response. False for Query, True for Response.
        /// </summary>
        public bool QueryResponse
        {
            get => _flags.GetFlag(15);
            set => _flags = _flags.SetFlag(15, value);
        }

        /// <summary>
        ///     Gets or sets the record operation code flag.
        /// </summary>
        public OperationCode OpCode
        {
            get => (OperationCode)_flags.GetFlag(11, 4);
            set => _flags = _flags.SetFlag(11, 4, (ushort)value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the record is an authoritative answer.
        /// </summary>
        public bool AuthoritativeAnswer
        {
            get => _flags.GetFlag(10);
            set => _flags = _flags.SetFlag(10, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the truncation flag is set.
        /// </summary>
        public bool Truncation
        {
            get => _flags.GetFlag(9);
            set => _flags = _flags.SetFlag(9, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion flag is set.
        /// </summary>
        public bool Recursion
        {
            get => _flags.GetFlag(8);
            set => _flags = _flags.SetFlag(8, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion available flag is set.
        /// </summary>
        public bool RecursionAvailable
        {
            get => _flags.GetFlag(7);
            set => _flags = _flags.SetFlag(7, value);
        }

        /// <summary>
        ///     Gets or sets a record reserved flag.
        /// </summary>
        public ushort Z
        {
            get => _flags.GetFlag(4, 3);
            set => _flags = _flags.SetFlag(4, 3, value);
        }

        /// <summary>
        ///     Gets or sets the record response code.
        /// </summary>
        public ResponseCode ResponseCode
        {
            get => (ResponseCode)_flags.GetFlag(0, 4);
            set => _flags = _flags.SetFlag(0, 4, (ushort)value);
        }

        /// <summary>
        ///     Gets the header as a byte array.
        /// </summary>
        /// <returns>Byte array of the header data.</returns>
        public IEnumerable<byte> GetBytes()
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
    }
}
