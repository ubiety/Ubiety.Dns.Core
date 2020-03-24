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
    /// <summary>
    ///     DNS request.
    /// </summary>
    public class Request
    {
        private readonly List<Question> _questions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Request" /> class.
        /// </summary>
        public Request()
        {
            Header = new Header
            {
                OpCode = OperationCode.Query,
                QuestionCount = 0,
            };

            _questions = new List<Question>();
        }

        /// <summary>
        ///     Gets the DNS record header.
        /// </summary>
        public Header Header { get; }

        /// <summary>
        ///     Gets the request as a byte array.
        /// </summary>
        /// <returns>Byte array of the data.</returns>
        public byte[] GetData()
        {
            var data = new List<byte>();
            Header.QuestionCount = (ushort)_questions.Count;
            data.AddRange(Header.GetData());
            foreach (var q in _questions)
            {
                data.AddRange(q.GetData());
            }

            return data.ToArray();
        }

        /// <summary>
        ///     Add a question to the request.
        /// </summary>
        /// <param name="question">Question to add to the request.</param>
        public void AddQuestion(Question question)
        {
            _questions.Add(question);
        }
    }
}