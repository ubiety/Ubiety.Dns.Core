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

namespace Ubiety.Dns.Core;

/// <summary>
/// Represents a DNS request with associated header and questions.
/// </summary>
/// <remarks>
/// The <see cref="Request"/> class provides functionality to create and manage DNS requests.
/// It contains a header and a collection of questions that make up the DNS query.
/// </remarks>
public class Request
{
    private readonly List<Question> _questions;

    /// <summary>
    /// Initializes a new instance of the <see cref="Request"/> class.
    /// </summary>
    /// <remarks>
    /// The constructor sets up a default DNS request, initializing the header for a query and preparing the collection of questions.
    /// </remarks>
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

    /// <summary>
    /// Gets the header information of the DNS request.
    /// </summary>
    /// <remarks>
    /// The header contains metadata such as the query ID, flags, and counts for different sections
    /// of the DNS request or response. It is used to manage and track the state of the DNS operation.
    /// </remarks>
    public Header Header { get; }

    /// <summary>
    /// Converts the DNS request, including its header and questions, into a byte array representation.
    /// </summary>
    /// <returns>A byte array containing the serialized form of the DNS request.</returns>
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

    /// <summary> Adds a new question to the DNS request. </summary>
    /// <param name="question">The DNS question to be added to the request.</param>
    public void AddQuestion(Question question)
    {
        Header.QuestionCount++;
        _questions.Add(question);
    }
}
