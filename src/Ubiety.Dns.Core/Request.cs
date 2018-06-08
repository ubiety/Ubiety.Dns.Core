using System;
using System.Collections.Generic;
using System.Text;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS request
    /// </summary>
    public class Request
    {
        private readonly List<Question> questions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Request" /> class
        /// </summary>
        public Request()
        {
            this.Header = new Header();
            this.Header.OPCODE = OperationCode.Query;
            this.Header.QuestionCount = 0;

            this.questions = new List<Question>();
        }

        /// <summary>
        ///     Gets or sets the DNS record header
        /// </summary>
        public Header Header { get; set; }

        /// <summary>
        ///     Gets the request as a byte array
        /// </summary>
        /// <returns>Byte array of the data</returns>
        public Byte[] GetData()
        {
                List<byte> data = new List<byte>();
                this.Header.QuestionCount = (ushort)this.questions.Count;
                data.AddRange(this.Header.GetData());
                foreach (Question q in this.questions)
                {
                    data.AddRange(q.GetData());
                }

                return data.ToArray();
        }

        /// <summary>
        ///     Add a question to the request
        /// </summary>
        /// <param name="question">Question to add to the request</param>
        public void AddQuestion(Question question)
        {
            this.questions.Add(question);
        }
    }
}
