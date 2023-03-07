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

using Serilog;

using Ubiety.Dns.Core.Common.Extensions;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS response.
    /// </summary>
    public class Response
    {
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        /// <param name="timedOut">Sets whether the response timed out or not.</param>
        public Response(ILogger logger, bool timedOut)
        {
            _logger = logger;
            Questions = new List<Question>();
            Answers = new List<AnswerResourceRecord>();
            Authorities = new List<AuthorityResourceRecord>();
            Additional = new List<AdditionalResourceRecord>();

            Server = new IPEndPoint(0, 0);
            MessageSize = 0;
            TimeStamp = DateTime.Now;
            Header = new Header();
            TimedOut = timedOut;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        public Response(ILogger logger)
            : this(logger, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        /// <param name="server">
        ///     <see cref="IPEndPoint" /> of the DNS server that responded to the query.
        /// </param>
        /// <param name="data">   <see cref="byte" /> array of the response data. </param>
        public Response(ILogger logger, IPEndPoint server, byte[] data)
            : this(logger)
        {
            _logger.Debug("Received information from server");
            data = data.ThrowIfNull(nameof(data));
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
        ///     Gets the list of question records.
        /// </summary>
        public List<Question> Questions { get; }

        /// <summary>
        ///     Gets the list of answer resource records.
        /// </summary>
        public List<AnswerResourceRecord> Answers { get; }

        /// <summary>
        ///     Gets the list of authority resource records.
        /// </summary>
        public List<AuthorityResourceRecord> Authorities { get; }

        /// <summary>
        ///     Gets the list of additional resource records.
        /// </summary>
        public List<AdditionalResourceRecord> Additional { get; }

        /// <summary>
        ///     Gets the response header.
        /// </summary>
        public Header Header { get; }

        /// <summary>
        ///     Gets a value indicating whether the response timed out or not.
        /// </summary>
        public bool TimedOut { get; }

        /// <summary>
        ///     Gets or sets the size of the message.
        /// </summary>
        public int MessageSize { get; set; }

        /// <summary>
        ///     Gets the timestamp when cached.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        ///     Gets the <see cref="IPEndPoint" /> of the DNS server that responded.
        /// </summary>
        public IPEndPoint Server { get; }

        /// <summary>
        ///     Gets a list of resource records in the <see cref="Response" />.
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

        /// <summary> Gets the records. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <returns> The records. </returns>
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
}
