using System.Collections.Generic;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS request
    /// </summary>
    public class Request
    {
        private readonly List<Question> _questions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Request" /> class
        /// </summary>
        public Request()
        {
            Header = new Header
            {
                OpCode = OperationCode.Query,
                QuestionCount = 0
            };

            _questions = new List<Question>();
        }

        /// <summary>
        ///     Gets the DNS record header
        /// </summary>
        public Header Header { get; }

        /// <summary>
        ///     Gets the request as a byte array
        /// </summary>
        /// <returns>Byte array of the data</returns>
        public byte[] GetData()
        {
            var data = new List<byte>();
            Header.QuestionCount = (ushort)_questions.Count;
            data.AddRange(Header.GetData());
            foreach (var q in _questions)
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
            _questions.Add(question);
        }
    }
}