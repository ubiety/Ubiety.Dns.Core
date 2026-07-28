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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Ubiety.Dns.Core.Records;
using Ubiety.Logging.Core;

namespace Ubiety.Dns.Core;

/// <summary>
/// Represents a DNS response received from a DNS server. This class contains
/// details about the DNS response, such as questions, answers, authorities,
/// additional records, and metadata.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="Response" /> class.
/// </remarks>
/// <param name="timedOut">Sets whether the response timed out or not.</param>
public class Response(bool timedOut)
{
    private readonly IUbietyLogger _logger = UbietyLogger.Get<Response>();

    /// <summary>
    ///     Initializes a new instance of the <see cref="Response" /> class.
    /// </summary>
    public Response()
        : this(false)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Response" /> class.
    /// </summary>
    /// <param name="server">
    ///     <see cref="IPEndPoint" /> of the DNS server that responded to the query.
    /// </param>
    /// <param name="data">   <see cref="byte" /> array of the response data. </param>
    public Response(IPEndPoint server, byte[] data)
        : this()
    {
        ArgumentNullException.ThrowIfNull(data);
        _logger.Debug("Received information from server");
        Server = server;
        MessageSize = data.Length;
        var reader = new RecordReader(data);

        Header = new Header(reader);

        for (var i = 0; i < Header.QuestionCount; i++)
        {
            _logger.Debug("Adding questions...");
            Questions.Add(new Question(reader));
        }

        for (var i = 0; i < Header.AnswerCount; i++)
        {
            _logger.Debug("Adding answers...");
            Answers.Add(new AnswerResourceRecord(reader));
        }

        for (var i = 0; i < Header.NameserverCount; i++)
        {
            Authorities.Add(new AuthorityResourceRecord(reader));
        }

        for (var i = 0; i < Header.AdditionalRecordsCount; i++)
        {
            Additional.Add(new AdditionalResourceRecord(reader));
        }
    }

    /// <summary>
    /// Gets the list of question resource records.
    /// </summary>
    public List<Question> Questions { get; } = [];

    /// <summary>
    /// Gets the list of answer resource records.
    /// </summary>
    public List<AnswerResourceRecord> Answers { get; } = [];

    /// <summary>
    /// Gets the list of authority resource records.
    /// </summary>
    public List<AuthorityResourceRecord> Authorities { get; } = [];

    /// <summary>
    /// Gets the list of additional resource records.
    /// </summary>
    public List<AdditionalResourceRecord> Additional { get; } = [];

    /// <summary>
    /// Gets the header information of the DNS response. The header contains metadata
    /// such as ID, flags, question count, and record counts associated with the DNS response.
    /// </summary>
    public Header Header { get; } = new Header();

    /// <summary>
    /// Gets a value indicating whether the DNS response timed out.
    /// </summary>
    public bool TimedOut { get; } = timedOut;

    /// <summary>
    /// Gets or sets the size of the DNS message in bytes.
    /// </summary>
    public int MessageSize { get; set; } = 0;

    /// <summary>
    /// Gets the UTC timestamp indicating when the response was received or created.
    /// </summary>
    /// <remarks>
    /// This is UTC so that record expiry stays correct across a daylight saving transition.
    /// </remarks>
    public DateTime TimeStamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the IP endpoint of the DNS server that provided the response.
    /// </summary>
    public IPEndPoint Server { get; } = new IPEndPoint(0, 0);

    /// <summary>
    /// Gets the collection of all resource records, including answers, authorities, and additional records.
    /// </summary>
    public IEnumerable<ResourceRecord> ResourceRecords
    {
        get
        {
            var list = Answers.Cast<ResourceRecord>().ToList();
            list.AddRange(Authorities);

            list.AddRange(Additional);

            return list;
        }
    }

    /// <summary>
    /// Retrieves a list of DNS records of a specific type from the response.
    /// </summary>
    /// <typeparam name="T">The type of DNS records to retrieve, derived from <see cref="Record"/>.</typeparam>
    /// <returns>A list of DNS records of the specified type found in the response.</returns>
    public List<T> GetRecords<T>()
        where T : Record
    {
        var list = new List<T>();
        foreach (var resource in Answers)
        {
            if (resource.Record is T record)
            {
                list.Add(record);
            }
        }

        return list;
    }
}
