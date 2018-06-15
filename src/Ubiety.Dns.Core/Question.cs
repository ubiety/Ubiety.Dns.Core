using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        private String questionName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="questionName">Query name</param>
        /// <param name="questionType">Question type</param>
        /// <param name="questionClass">Question class</param>
        public Question(String questionName, QuestionType questionType, QuestionClass questionClass)
        {
            this.QuestionName = questionName;
            this.QuestionType = questionType;
            this.QuestionClass = questionClass;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record</param>
        public Question(RecordReader rr)
        {
            this.QuestionName = rr.ReadDomainName();
            this.QuestionType = (QuestionType)rr.ReadUInt16();
            this.QuestionClass = (QuestionClass)rr.ReadUInt16();
        }

        /// <summary>
        ///     Gets or sets the question name
        /// </summary>
        public String QuestionName
        {
            get
            {
                return this.questionName;
            }

            set
            {
                this.questionName = value;
                if (!this.questionName.EndsWith(".", StringComparison.InvariantCulture))
                {
                    this.questionName += ".";
                }
            }
        }

        /// <summary>
        ///     Gets or sets the query type
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        ///     Gets or sets the query class
        /// </summary>
        public QuestionClass QuestionClass { get; set; }

        /// <summary>
        ///     String representation of the question
        /// </summary>
        /// <returns>String of the question</returns>
        public override String ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0,-32}\t{1}\t{2}", this.QuestionName, this.QuestionClass, this.QuestionType);
        }

        /// <summary>
        ///     Gets the question as a byte array
        /// </summary>
        /// <returns>Byte array of the question data</returns>
        public Byte[] GetData()
        {
            List<Byte> data = new List<Byte>();
            data.AddRange(WriteName(this.QuestionName));
            data.AddRange(WriteShort((UInt16)this.QuestionType));
            data.AddRange(WriteShort((UInt16)this.QuestionClass));
            return data.ToArray();
        }

        private static Byte[] WriteName(String src)
        {
            if (!src.EndsWith(".", StringComparison.InvariantCulture))
            {
                src += ".";
            }

            if (src == ".")
            {
                return new Byte[1];
            }

            StringBuilder sb = new StringBuilder();
            Int32 intI, intJ, intLen = src.Length;
            sb.Append('\0');
            for (intI = 0, intJ = 0; intI < intLen; intI++, intJ++)
            {
                sb.Append(src[intI]);
                if (src[intI] == '.')
                {
                    sb[intI - intJ] = (Char)(intJ & 0xff);
                    intJ = -1;
                }
            }

            sb[sb.Length - 1] = '\0';
            return System.Text.Encoding.ASCII.GetBytes(sb.ToString());
        }

        private static Byte[] WriteShort(UInt16 sValue)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)sValue));
        }
    }
}
