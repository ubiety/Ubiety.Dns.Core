using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Heijden.DNS;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS Question record
    /// </summary>
    public class Question
    {
        private string m_QName;

        /// <summary>
        ///     Gets or sets the question name
        /// </summary>
        public string QName
        {
            get
            {
                return m_QName;
            }
            set
            {
                m_QName = value;
                if (!m_QName.EndsWith("."))
                    m_QName += ".";
            }
        }

        /// <summary>
        ///     Gets or sets the query type
        /// </summary>
        public QType QType;

        /// <summary>
        ///     Gets or sets the query class
        /// </summary>
        public QClass QClass;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="QName">Query name</param>
        /// <param name="QType">Query type</param>
        /// <param name="QClass">Query class</param>
        public Question(string QName, QType QType, QClass QClass)
        {
            this.QName = QName;
            this.QType = QType;
            this.QClass = QClass;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record</param>
        public Question(RecordReader rr)
        {
            QName = rr.ReadDomainName();
            QType = (QType)rr.ReadUInt16();
            QClass = (QClass)rr.ReadUInt16();
        }

        private byte[] WriteName(string src)
        {
            if (!src.EndsWith("."))
                src += ".";

            if (src == ".")
                return new byte[1];

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

        /// <summary>
        ///     Gets the question as a byte array
        /// </summary>
        public byte[] Data
        {
            get
            {
                List<byte> data = new List<byte>();
                data.AddRange(WriteName(QName));
                data.AddRange(WriteShort((ushort)QType));
                data.AddRange(WriteShort((ushort)QClass));
                return data.ToArray();
            }
        }

        private byte[] WriteShort(ushort sValue)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)sValue));
        }

        /// <summary>
        ///     String representation of the question
        /// </summary>
        /// <returns>String representation of the question</returns>
        public override string ToString()
        {
            return string.Format("{0,-32}\t{1}\t{2}", QName, QClass, QType);
        }
    }
}
