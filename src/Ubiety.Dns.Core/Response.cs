using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS response
    /// </summary>
    public class Response
    {
        /// <summary>
        /// List of Question records
        /// </summary>
        public List<Question> Questions;
        /// <summary>
        /// List of AnswerRR records
        /// </summary>
        public List<AnswerRR> Answers;
        /// <summary>
        /// List of AuthorityRR records
        /// </summary>
        public List<AuthorityRR> Authorities;
        /// <summary>
        /// List of AdditionalRR records
        /// </summary>
        public List<AdditionalRR> Additionals;

        /// <summary>
        /// </summary>
        public Header header;

        /// <summary>
        /// Error message, empty when no error
        /// </summary>
        public string Error;

        /// <summary>
        /// The Size of the message
        /// </summary>
        public int MessageSize;

        /// <summary>
        /// TimeStamp when cached
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        /// Server which delivered this response
        /// </summary>
        public IPEndPoint Server;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class
        /// </summary>
        public Response()
        {
            this.Questions = new List<Question>();
            this.Answers = new List<AnswerRR>();
            this.Authorities = new List<AuthorityRR>();
            this.Additionals = new List<AdditionalRR>();

            this.Server = new IPEndPoint(0,0);
            this.Error = "";
            this.MessageSize = 0;
            this.TimeStamp = DateTime.Now;
            this.header = new Header();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Response" /> class
        /// </summary>
        /// <param name="iPEndPoint">Address of the response</param>
        /// <param name="data">Response data</param>
        public Response(IPEndPoint iPEndPoint, byte[] data)
        {
            this.Error = "";
            this.Server = iPEndPoint;
            this.TimeStamp = DateTime.Now;
            this.MessageSize = data.Length;
            RecordReader rr = new RecordReader(data);

            this.Questions = new List<Question>();
            this.Answers = new List<AnswerRR>();
            this.Authorities = new List<AuthorityRR>();
            this.Additionals = new List<AdditionalRR>();

            this.header = new Header(rr);

            for (int intI = 0; intI < header.QuestionCount; intI++)
            {
                this.Questions.Add(new Question(rr));
            }

            for (int intI = 0; intI < header.AnswerCount; intI++)
            {
                this.Answers.Add(new AnswerRR(rr));
            }

            for (int intI = 0; intI < header.NameserverCount; intI++)
            {
                this.Authorities.Add(new AuthorityRR(rr));
            }
            for (int intI = 0; intI < header.AdditionalRecordsCount; intI++)
            {
                this.Additionals.Add(new AdditionalRR(rr));
            }
        }

        /// <summary>
        /// List of RecordMX in Response.Answers
        /// </summary>
        public RecordMX[] RecordsMX
        {
            get
            {
                List<RecordMX> list = new List<RecordMX>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordMX record = answerRR.Record as RecordMX;
                    if(record!=null)
                        list.Add(record);
                }
                list.Sort();
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordTXT in Response.Answers
        /// </summary>
        public RecordTXT[] RecordsTXT
        {
            get
            {
                List<RecordTXT> list = new List<RecordTXT>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordTXT record = answerRR.Record as RecordTXT;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordA in Response.Answers
        /// </summary>
        public RecordA[] RecordsA
        {
            get
            {
                List<RecordA> list = new List<RecordA>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordA record = answerRR.Record as RecordA;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordPTR in Response.Answers
        /// </summary>
        public RecordPTR[] RecordsPTR
        {
            get
            {
                List<RecordPTR> list = new List<RecordPTR>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordPTR record = answerRR.Record as RecordPTR;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordCNAME in Response.Answers
        /// </summary>
        public RecordCNAME[] RecordsCNAME
        {
            get
            {
                List<RecordCNAME> list = new List<RecordCNAME>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordCNAME record = answerRR.Record as RecordCNAME;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordAAAA in Response.Answers
        /// </summary>
        public RecordAAAA[] RecordsAAAA
        {
            get
            {
                List<RecordAAAA> list = new List<RecordAAAA>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordAAAA record = answerRR.Record as RecordAAAA;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordNS in Response.Answers
        /// </summary>
        public RecordNS[] RecordsNS
        {
            get
            {
                List<RecordNS> list = new List<RecordNS>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordNS record = answerRR.Record as RecordNS;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordSOA in Response.Answers
        /// </summary>
        public RecordSOA[] RecordsSOA
        {
            get
            {
                List<RecordSOA> list = new List<RecordSOA>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordSOA record = answerRR.Record as RecordSOA;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// List of RecordCERT in Response.Answers
        /// </summary>
        public RecordCERT[] RecordsCERT
        {
            get
            {
                List<RecordCERT> list = new List<RecordCERT>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordCERT record = answerRR.Record as RecordCERT;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// </summary>
        public RecordSRV[] RecordsSRV
        {
            get
            {
                List<RecordSRV> list = new List<RecordSRV>();
                foreach (AnswerRR answerRR in this.Answers)
                {
                    RecordSRV record = answerRR.Record as RecordSRV;
                    if (record != null)
                        list.Add(record);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// </summary>
        public RR[] RecordsRR
        {
            get
            {
                List<RR> list = new List<RR>();
                foreach (RR rr in this.Answers)
                {
                    list.Add(rr);
                }
                foreach (RR rr in this.Answers)
                {
                    list.Add(rr);
                }
                foreach (RR rr in this.Authorities)
                {
                    list.Add(rr);
                }
                foreach (RR rr in this.Additionals)
                {
                    list.Add(rr);
                }
                return list.ToArray();
            }
        }


    }
}
