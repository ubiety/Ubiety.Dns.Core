/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS Question record.
    /// </summary>
    public sealed class Question : IEquatable<Question>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class.
        /// </summary>
        /// <param name="domainName">Domain name to look up with the question.</param>
        /// <param name="questionType">Question type.</param>
        /// <param name="questionClass">Question class.</param>
        public Question(string domainName, QuestionType questionType, QuestionClass questionClass)
        {
            if (!domainName.ThrowIfNull(nameof(domainName)).EndsWith(".", StringComparison.InvariantCulture))
            {
                domainName += ".";
            }

            DomainName = domainName;
            QuestionType = questionType;
            QuestionClass = questionClass;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Question" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record.</param>
        internal Question(RecordReader rr)
        {
            DomainName = rr.ReadDomainName();
            QuestionType = (QuestionType)rr.ReadUInt16();
            QuestionClass = (QuestionClass)rr.ReadUInt16();
        }

        /// <summary>
        ///     Gets the question name.
        /// </summary>
        public string DomainName { get; }

        /// <summary>
        ///     Gets the query type.
        /// </summary>
        public QuestionType QuestionType { get; }

        /// <summary>
        ///     Gets the query class.
        /// </summary>
        public QuestionClass QuestionClass { get; }

        /// <inheritdoc cref="IEquatable{T}" />
        public static bool operator ==(Question left, Question right)
        {
            return Equals(left, right);
        }

        /// <inheritdoc cref="IEquatable{T}" />
        public static bool operator !=(Question left, Question right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc cref="IEquatable{T}" />
        public bool Equals(Question other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(DomainName, other.DomainName, StringComparison.InvariantCultureIgnoreCase) &&
                   QuestionType == other.QuestionType && QuestionClass == other.QuestionClass;
        }

        /// <inheritdoc cref="IEquatable{T}" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Question)obj);
        }

        /// <summary>
        ///     String representation of the question.
        /// </summary>
        /// <returns>String of the question.</returns>
        public override string ToString()
        {
            return $"{DomainName,-32}\t{QuestionClass}\t{QuestionType}";
        }

        /// <summary>
        ///     Gets the question as a byte array.
        /// </summary>
        /// <returns>Byte array of the question data.</returns>
        public IEnumerable<byte> GetData()
        {
            var data = new List<byte>();
            data.AddRange(WriteName(DomainName));
            data.AddRange(((ushort)QuestionType).GetBytes());
            data.AddRange(((ushort)QuestionClass).GetBytes());
            return data.ToArray();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(DomainName);
                hashCode = (hashCode * 397) ^ (int)QuestionType;
                hashCode = (hashCode * 397) ^ (int)QuestionClass;
                return hashCode;
            }
        }

        private static IEnumerable<byte> WriteName(string src)
        {
            if (!src.EndsWith(".", StringComparison.InvariantCulture))
            {
                src += ".";
            }

            if (src == ".")
            {
                return new byte[1];
            }

            var sb = new StringBuilder();
            sb.Append('\0');
            for (int i = 0, j = 0; i < src.Length; i++, j++)
            {
                sb.Append(src[i]);
                if (src[i] != '.')
                {
                    continue;
                }

                sb[i - j] = (char)(j & 0xff);
                j = -1;
            }

            sb[sb.Length - 1] = '\0';
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
    }
}