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
using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core;

/// <summary>
/// Represents the DNS message header, which contains metadata about a DNS request or response.
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
    /// Gets or sets the identifier for the DNS message header,
    /// used to match requests with responses.
    /// </summary>
    public ushort Id { get; set; }

    /// <summary>
    /// Gets or sets the number of questions in the DNS message.
    /// This represents the count of entries in the question section of the DNS message.
    /// </summary>
    public ushort QuestionCount { get; set; }

    /// <summary>
    /// Gets or sets the number of answer records in the DNS message.
    /// This corresponds to the count of resource records in the Answer section of the message.
    /// </summary>
    public ushort AnswerCount { get; set; }

    /// <summary>
    /// Gets or sets the count of authoritative nameservers in a DNS response.
    /// This property is used to track the number of resource records in the
    /// authority section of a DNS message.
    /// </summary>
    public ushort NameserverCount { get; set; }

    /// <summary>
    /// Gets or sets the count of additional resource records in the DNS message.
    /// </summary>
    public ushort AdditionalRecordsCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message is a query (false) or a response (true).
    /// </summary>
    public bool QueryResponse
    {
        get => _flags.GetFlag(15);
        set => _flags = _flags.SetFlag(15, value);
    }

    /// <summary>
    /// Gets or sets the operation code (OpCode) of the DNS message,
    /// which indicates the type of query or operation being performed.
    /// </summary>
    public OperationCode OpCode
    {
        get => (OperationCode)_flags.GetFlag(11, 4);
        set => _flags = _flags.SetFlag(11, 4, (ushort)value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the responding server is authoritative for the domain name in the query.
    /// </summary>
    public bool AuthoritativeAnswer
    {
        get => _flags.GetFlag(10);
        set => _flags = _flags.SetFlag(10, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the DNS message has been truncated.
    /// Truncation occurs when the message size exceeds the specified limit.
    /// </summary>
    public bool Truncation
    {
        get => _flags.GetFlag(9);
        set => _flags = _flags.SetFlag(9, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether recursion is desired by the client
    /// or allowed in the server for the DNS operation.
    /// </summary>
    public bool Recursion
    {
        get => _flags.GetFlag(8);
        set => _flags = _flags.SetFlag(8, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether recursion is available for the DNS query.
    /// This flag is used in DNS responses to signify if the server supports recursive queries.
    /// </summary>
    public bool RecursionAvailable
    {
        get => _flags.GetFlag(7);
        set => _flags = _flags.SetFlag(7, value);
    }

    /// <summary>
    /// Gets or sets the reserved bit field in the DNS message header.
    /// This field is reserved for future use and must be set to zero as per RFC specifications.
    /// </summary>
    public ushort Z
    {
        get => _flags.GetFlag(4, 3);
        set => _flags = _flags.SetFlag(4, 3, value);
    }

    /// <summary>
    /// Gets or sets the response code, indicating the result of the DNS query processing.
    /// This code defines the outcome of the DNS operation, such as success, failure, or specific error categories.
    /// </summary>
    public ResponseCode ResponseCode
    {
        get => (ResponseCode)_flags.GetFlag(0, 4);
        set => _flags = _flags.SetFlag(0, 4, (ushort)value);
    }

    /// <summary>
    /// Converts the header properties into a sequence of bytes for serialization.
    /// </summary>
    /// <returns>A sequence of bytes representing the header data.</returns>
    public IEnumerable<byte> GetBytes()
    {
        return [..Id.GetBytes(), .._flags.GetBytes(), ..QuestionCount.GetBytes(), ..AnswerCount.GetBytes(), ..NameserverCount.GetBytes(), ..AdditionalRecordsCount.GetBytes()];
    }
}
