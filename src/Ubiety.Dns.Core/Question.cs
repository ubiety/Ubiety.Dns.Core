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
        private string questionName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="queryName">Query name</param>
        /// <param name="questionType">Question type</param>
        /// <param name="queryClass">Query class</param>
        public Question(string queryName, QuestionType questionType, QClass queryClass)
        {
            this.QName = queryName;
            this.QType = questionType;
            this.QClass = queryClass;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record</param>
        public Question(RecordReader rr)
        {
            this.QName = rr.ReadDomainName();
            this.QType = (QuestionType)rr.ReadUInt16();
            this.QClass = (QClass)rr.ReadUInt16();
        }

        /// <summary>
        ///     Gets or sets the question name
        /// </summary>
        public string QName
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
        public QuestionType QType { get; set; }

        /// <summary>
        ///     Gets or sets the query class
        /// </summary>
        public QClass QClass { get; set; }

        /// <summary>
        ///     String representation of the question
        /// </summary>
        /// <returns>String of the question</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0,-32}\t{1}\t{2}", this.QName, this.QClass, this.QType);
        }

        /// <summary>
        ///     Gets the question as a byte array
        /// </summary>
        /// <returns>Byte array of the question data</returns>
        public byte[] GetData()
        {
            List<byte> data = new List<byte>();
            data.AddRange(WriteName(this.QName));
            data.AddRange(WriteShort((ushort)this.QType));
            data.AddRange(WriteShort((ushort)this.QClass));
            return data.ToArray();
        }

        private static byte[] WriteName(string src)
        {
            if (!src.EndsWith(".", StringComparison.InvariantCulture))
            {
                src += ".";
            }

            if (src == ".")
            {
                return new byte[1];
            }

            StringBuilder sb = new StringBuilder();
            int intI, intJ, intLen = src.Length;
            sb.Append('\0');
            for (intI = 0, intJ = 0; intI < intLen; intI++, intJ++)
            {
                sb.Append(src[intI]);
                if (src[intI] == '.')
                {
                    sb[intI - intJ] = (char)(intJ & 0xff);
                    intJ = -1;
                }
            }

            sb[sb.Length - 1] = '\0';
            return System.Text.Encoding.ASCII.GetBytes(sb.ToString());
        }

        private static byte[] WriteShort(ushort sValue)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)sValue));
        }
    }
}
