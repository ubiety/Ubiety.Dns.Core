using System;
using System.Collections.Generic;
using System.Text;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS request
    /// </summary>
    public class Request
    {
        /// <summary>
        ///     DNS record header
        /// </summary>
        public Header header;

        private readonly List<Question> questions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Request" /> class
        /// </summary>
        public Request()
        {
            this.header = new Header();
            this.header.OPCODE = OPCode.Query;
            this.header.QuestionCount = 0;

            this.questions = new List<Question>();
        }

        /// <summary>
        ///     Add a question to the request
        /// </summary>
        /// <param name="question">Question to add to the request</param>
        public void AddQuestion(Question question)
        {
            this.questions.Add(question);
        }

        /// <summary>
        ///     Gets or sets the request as a byte array
        /// </summary>
        public byte[] Data
        {
            get
            {
                List<byte> data = new List<byte>();
                this.header.QuestionCount = (ushort)this.questions.Count;
                data.AddRange(this.header.Data);
                foreach (Question q in this.questions)
                    data.AddRange(q.Data);
                return data.ToArray();
            }
        }
    }
}
