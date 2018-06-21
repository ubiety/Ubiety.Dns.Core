using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS Question record
    /// </summary>
    public class Question
    {
        private string _questionName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="questionName">Query name</param>
        /// <param name="questionType">Question type</param>
        /// <param name="questionClass">Question class</param>
        public Question(string questionName, QuestionType questionType, QuestionClass questionClass)
        {
            QuestionName = questionName;
            QuestionType = questionType;
            QuestionClass = questionClass;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record</param>
        public Question(RecordReader rr)
        {
            QuestionName = rr.ReadDomainName();
            QuestionType = (QuestionType)rr.ReadUInt16();
            QuestionClass = (QuestionClass)rr.ReadUInt16();
        }

        /// <summary>
        ///     Gets the question name
        /// </summary>
        public String QuestionName
        {
            get => _questionName;

            private set
            {
                _questionName = value;
                if (!_questionName.EndsWith(".", StringComparison.InvariantCulture))
                {
                    _questionName += ".";
                }
            }
        }

        /// <summary>
        ///     Gets the query type
        /// </summary>
        public QuestionType QuestionType { get; }

        /// <summary>
        ///     Gets the query class
        /// </summary>
        public QuestionClass QuestionClass { get; }

        /// <summary>
        ///     String representation of the question
        /// </summary>
        /// <returns>String of the question</returns>
        public override String ToString()
        {
            return $"{QuestionName, -32}\t{QuestionClass}\t{QuestionType}";
        }

        /// <summary>
        ///     Gets the question as a byte array
        /// </summary>
        /// <returns>Byte array of the question data</returns>
        public IEnumerable<Byte> GetData()
        {
            var data = new List<Byte>();
            data.AddRange(WriteName(QuestionName));
            data.AddRange(WriteShort((UInt16)QuestionType));
            data.AddRange(WriteShort((UInt16)QuestionClass));
            return data.ToArray();
        }

        private static IEnumerable<Byte> WriteName(String src)
        {
            if (!src.EndsWith(".", StringComparison.InvariantCulture))
            {
                src += ".";
            }

            if (src == ".")
            {
                return new Byte[1];
            }

            var sb = new StringBuilder();
            Int32 intI, intJ, intLen = src.Length;
            sb.Append('\0');
            for (intI = 0, intJ = 0; intI < intLen; intI++, intJ++)
            {
                sb.Append(src[intI]);
                if (src[intI] != '.')
                {
                    continue;
                }

                sb[intI - intJ] = (Char)(intJ & 0xff);
                intJ = -1;
            }

            sb[sb.Length - 1] = '\0';
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        private static Byte[] WriteShort(UInt16 value)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)value));
        }
    }
}