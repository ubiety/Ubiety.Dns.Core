using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Dns.Core.Records.Mail;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS response
    /// </summary>
    public class Response
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class
        /// </summary>
        public Response()
        {
            this.Questions = new List<Question>();
            this.Answers = new List<AnswerRR>();
            this.Authorities = new List<AuthorityRR>();
            this.Additionals = new List<AdditionalRR>();

            this.Server = new IPEndPoint(0, 0);
            this.Error = String.Empty;
            this.MessageSize = 0;
            this.TimeStamp = DateTime.Now;
            this.Header = new Header();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class
        /// </summary>
        /// <param name="iPEndPoint">Address of the response</param>
        /// <param name="data">Response data</param>
        public Response(IPEndPoint iPEndPoint, Byte[] data)
        {
            this.Error = String.Empty;
            this.Server = iPEndPoint;
            this.TimeStamp = DateTime.Now;
            this.MessageSize = data.Length;
            RecordReader rr = new RecordReader(data);

            this.Questions = new List<Question>();
            this.Answers = new List<AnswerRR>();
            this.Authorities = new List<AuthorityRR>();
            this.Additionals = new List<AdditionalRR>();

            this.Header = new Header(rr);

            for (Int32 i = 0; i < this.Header.QuestionCount; i++)
            {
                this.Questions.Add(new Question(rr));
            }

            for (Int32 i = 0; i < this.Header.AnswerCount; i++)
            {
                this.Answers.Add(new AnswerRR(rr));
            }

            for (Int32 i = 0; i < this.Header.NameserverCount; i++)
            {
                this.Authorities.Add(new AuthorityRR(rr));
            }

            for (Int32 i = 0; i < this.Header.AdditionalRecordsCount; i++)
            {
                this.Additionals.Add(new AdditionalRR(rr));
            }
        }

        /// <summary>
        ///     Gets the list of question records
        /// </summary>
        public List<Question> Questions { get; }

        /// <summary>
        ///     Gets the list of answer resource records
        /// </summary>
        public List<AnswerRR> Answers { get; }

        /// <summary>
        ///     Gets the list of authority resource records
        /// </summary>
        public List<AuthorityRR> Authorities { get; }

        /// <summary>
        ///     Gets the list of additional resource records
        /// </summary>
        public List<AdditionalRR> Additionals { get; }

        /// <summary>
        ///     Gets or sets the response header
        /// </summary>
        public Header Header { get; set; }

        /// <summary>
        ///     Gets or sets the error message, empty when no error
        /// </summary>
        public String Error { get; set; }

        /// <summary>
        ///     Gets or sets the size of the message
        /// </summary>
        public Int32 MessageSize { get; set; }

        /// <summary>
        ///     Gets or sets the timestamp when cached
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     Gets or sets the server which delivered this response
        /// </summary>
        public IPEndPoint Server { get; set; }

        /// <summary>
        ///     Gets a list of MX records in the answers
        /// </summary>
        public List<RecordMx> RecordMx
        {
            get
            {
                List<RecordMx> list = new List<RecordMx>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordMx record = answerRR.Record as RecordMx;
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
        ///     Gets a list of TXT records in the answers
        /// </summary>
        public List<RecordTxt> RecordTxt
        {
            get
            {
                List<RecordTxt> list = new List<RecordTxt>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordTxt record = answerRR.Record as RecordTxt;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of A records in the answers
        /// </summary>
        public List<RecordA> RecordA
        {
            get
            {
                List<RecordA> list = new List<RecordA>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordA record = answerRR.Record as RecordA;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of PTR records from the answers
        /// </summary>
        public List<RecordPtr> RecordPtr
        {
            get
            {
                List<RecordPtr> list = new List<RecordPtr>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordPtr record = answerRR.Record as RecordPtr;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of CNAME records from the answers
        /// </summary>
        public List<RecordCname> RecordCname
        {
            get
            {
                List<RecordCname> list = new List<RecordCname>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordCname record = answerRR.Record as RecordCname;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of AAAA records in the answers
        /// </summary>
        public List<RecordAaaa> RecordAAAA
        {
            get
            {
                List<RecordAaaa> list = new List<RecordAaaa>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordAaaa record = answerRR.Record as RecordAaaa;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of NS records in the answers
        /// </summary>
        public List<RecordNs> RecordNS
        {
            get
            {
                List<RecordNs> list = new List<RecordNs>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordNs record = answerRR.Record as RecordNs;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of SOA records in the answers
        /// </summary>
        public List<RecordSoa> RecordSOA
        {
            get
            {
                List<RecordSoa> list = new List<RecordSoa>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordSoa record = answerRR.Record as RecordSoa;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of CERT records in the answers
        /// </summary>
        public List<RecordCert> RecordCERT
        {
            get
            {
                List<RecordCert> list = new List<RecordCert>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordCert record = answerRR.Record as RecordCert;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of SRV records in the answers
        /// </summary>
        public List<RecordSrv> RecordSRV
        {
            get
            {
                List<RecordSrv> list = new List<RecordSrv>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordSrv record = answerRR.Record as RecordSrv;
                    if (record != null)
                    {
                        list.Add(record);
                    }
                }

                return list;
            }
        }

        /// <summary>
        ///     Gets a list of resource records in the answers
        /// </summary>
        public List<ResourceRecord> ResourceRecords
        {
            get
            {
                List<ResourceRecord> list = new List<ResourceRecord>();
                foreach (ResourceRecord rr in this.Answers)
                {
                    list.Add(rr);
                }

                foreach (ResourceRecord rr in this.Answers)
                {
                    list.Add(rr);
                }

                foreach (ResourceRecord rr in this.Authorities)
                {
                    list.Add(rr);
                }

                foreach (ResourceRecord rr in this.Additionals)
                {
                    list.Add(rr);
                }

                return list;
            }
        }
    }
}
