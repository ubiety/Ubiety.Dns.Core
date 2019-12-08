/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Dns.Core.Records.Mail;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS response.
    /// </summary>
    public class Response
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        public Response()
        {
            Questions = new List<Question>();
            Answers = new List<AnswerRR>();
            Authorities = new List<AuthorityRR>();
            Additionals = new List<AdditionalRR>();

            Server = new IPEndPoint(0, 0);
            Error = string.Empty;
            MessageSize = 0;
            TimeStamp = DateTime.Now;
            Header = new Header();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class.
        /// </summary>
        /// <param name="iPEndPoint">Address of the response.</param>
        /// <param name="data">Response data.</param>
        public Response(IPEndPoint iPEndPoint, byte[] data)
        {
            data = data.ThrowIfNull(nameof(data));
            Error = string.Empty;
            Server = iPEndPoint;
            TimeStamp = DateTime.Now;
            MessageSize = data.Length;
            var rr = new RecordReader(data);

            Questions = new List<Question>();
            Answers = new List<AnswerRR>();
            Authorities = new List<AuthorityRR>();
            Additionals = new List<AdditionalRR>();

            Header = new Header(rr);

            for (var i = 0; i < Header.QuestionCount; i++)
            {
                Questions.Add(new Question(rr));
            }

            for (var i = 0; i < Header.AnswerCount; i++)
            {
                Answers.Add(new AnswerRR(rr));
            }

            for (var i = 0; i < Header.NameserverCount; i++)
            {
                Authorities.Add(new AuthorityRR(rr));
            }

            for (var i = 0; i < Header.AdditionalRecordsCount; i++)
            {
                Additionals.Add(new AdditionalRR(rr));
            }
        }

        /// <summary>
        ///     Gets the list of question records.
        /// </summary>
        public List<Question> Questions { get; }

        /// <summary>
        ///     Gets the list of answer resource records.
        /// </summary>
        public List<AnswerRR> Answers { get; }

        /// <summary>
        ///     Gets the list of authority resource records.
        /// </summary>
        public List<AuthorityRR> Authorities { get; }

        /// <summary>
        ///     Gets the list of additional resource records.
        /// </summary>
        public List<AdditionalRR> Additionals { get; }

        /// <summary>
        ///     Gets the response header.
        /// </summary>
        public Header Header { get; }

        /// <summary>
        ///     Gets or sets the error message, empty when no error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        ///     Gets or sets the size of the message.
        /// </summary>
        public int MessageSize { get; set; }

        /// <summary>
        ///     Gets the timestamp when cached.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        ///     Gets the server which delivered this response.
        /// </summary>
        public IPEndPoint Server { get; }

        /// <summary>
        ///     Gets a list of MX records in the answers.
        /// </summary>
        public List<RecordMx> RecordMx
        {
            get
            {
                var list = new List<RecordMx>();
                foreach (var rr in Answers)
                {
                    var record = rr.Record as RecordMx;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                list.Sort();
                return list;
            }
        }

        /// <summary>
        ///     Gets a list of TXT records in the <see cref="Response" />.
        /// </summary>
        public List<RecordTxt> RecordTxt
        {
            get
            {
                var list = new List<RecordTxt>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordTxt record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of A records in the <see cref="Response" />.
        /// </summary>
        public List<RecordA> RecordA
        {
            get
            {
                var list = new List<RecordA>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordA record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of PTR records from the <see cref="Response" />.
        /// </summary>
        public List<RecordPtr> RecordPtr
        {
            get
            {
                var list = new List<RecordPtr>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordPtr record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of CNAME records from the <see cref="Response" />.
        /// </summary>
        public List<RecordCname> RecordCname
        {
            get
            {
                var list = new List<RecordCname>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordCname record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of AAAA records in the <see cref="Response" />.
        /// </summary>
        public List<RecordAaaa> RecordAaaa
        {
            get
            {
                var list = new List<RecordAaaa>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordAaaa record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of NS records in the <see cref="Response" />.
        /// </summary>
        public List<RecordNs> RecordNs
        {
            get
            {
                var list = new List<RecordNs>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordNs record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of SOA records in the <see cref="Response" />.
        /// </summary>
        public List<RecordSoa> RecordSoa
        {
            get
            {
                var list = new List<RecordSoa>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordSoa record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of CERT records in the <see cref="Response" />.
        /// </summary>
        public List<RecordCert> RecordCert
        {
            get
            {
                var list = new List<RecordCert>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordCert record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of SRV records in the <see cref="Response" />.
        /// </summary>
        public List<RecordSrv> RecordSrv
        {
            get
            {
                var list = new List<RecordSrv>();
                foreach (var rr in Answers)
                {
                    if (rr.Record is RecordSrv record)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of resource records in the <see cref="Response" />.
        /// </summary>
        public IEnumerable<ResourceRecord> ResourceRecords
        {
            get
            {
                var list = Answers.Cast<ResourceRecord>().ToList();
                list.AddRange(Authorities);

                list.AddRange(Additionals);

                return list;
            }
        }
    }
}