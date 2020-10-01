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
