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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Ubiety.Dns.Core.Common.Extensions;
using Ubiety.Dns.Core.Records;
using Ubiety.Logging.Core;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS response.
    /// </summary>
    public class Response
    {
        private readonly IUbietyLogger _logger = UbietyLogger.Get<Response>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        /// <param name="timedOut">Sets whether the response timed out or not.</param>
        public Response(bool timedOut = false)
        {
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
        /// <param name="server"><see cref="IPEndPoint" /> of the DNS server that responded to the query.</param>
        /// <param name="data"><see cref="byte" /> array of the response data.</param>
        public Response(IPEndPoint server, byte[] data)
            : this()
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

        /// <summary>
        ///     Get a set of records from the answer.
        /// </summary>
        /// <typeparam name="T">Type of record to find.</typeparam>
        /// <returns>List of records.</returns>
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
