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

namespace Ubiety.Dns.Core
{
    /// <summary> A request. </summary>
    /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
    public class Request
    {
        private readonly List<Question> _questions;

        /// <summary> Initializes a new instance of the <see cref="Request" /> class. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        public Request()
        {
            Header = new Header
            {
                QueryResponse = false,
                OpCode = OperationCode.Query,
                QuestionCount = 0,
            };

            _questions = new List<Question>();
        }

        /// <summary> Gets the header. </summary>
        /// <value> The header. </value>
        public Header Header { get; }

        /// <summary> Gets the bytes. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <returns> An array of byte. </returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();
            data.AddRange(Header.GetBytes());
            foreach (var q in _questions)
            {
                data.AddRange(q.GetBytes());
            }

            return data.ToArray();
        }

        /// <summary> Adds a question. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <param name="question"> The question. </param>
        public void AddQuestion(Question question)
        {
            Header.QuestionCount++;
            _questions.Add(question);
        }
    }
}
