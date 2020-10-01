/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core
{
    /// <summary> A question. This class cannot be inherited. </summary>
    /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
    /// <seealso cref="IEquatable{Question}"/>
    public sealed class Question : IEquatable<Question>
    {
        /// <summary> Initializes a new instance of the <see cref="Question"/> class. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <param name="domainName">    Gets the question name. </param>
        /// <param name="questionType">  Gets the query type. </param>
        /// <param name="questionClass"> Gets the query class. </param>
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

        /// <summary> Initializes a new instance of the <see cref="Question" /> class. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <param name="reader"> <see cref="RecordReader" /> of the record. </param>
        internal Question(RecordReader reader)
        {
            DomainName = reader.ReadDomainName();
            QuestionType = (QuestionType)reader.ReadUInt16();
            QuestionClass = (QuestionClass)reader.ReadUInt16();
        }

        /// <summary> Gets the name of the domain. </summary>
        /// <value> The name of the domain. </value>
        public string DomainName { get; }

        /// <summary> Gets the type of the question. </summary>
        /// <value> The type of the question. </value>
        public QuestionType QuestionType { get; }

        /// <summary> Gets the question class. </summary>
        /// <value> The question class. </value>
        public QuestionClass QuestionClass { get; }

        /// <summary> Equality operator. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <param name="left">  The first instance to compare. </param>
        /// <param name="right"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator ==(Question left, Question right)
        {
            return Equals(left, right);
        }

        /// <summary> Inequality operator. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <param name="left">  The first instance to compare. </param>
        /// <param name="right"> The second instance to compare. </param>
        /// <returns> The result of the operation. </returns>
        public static bool operator !=(Question left, Question right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <param name="other"> An object to compare with this object. </param>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other">other</paramref>
        ///     parameter;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(Question other)
        {
            if (other is null)
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

        /// <summary> Determines whether the specified object is equal to the current object. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <param name="obj"> The object to compare with the current object. </param>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Question)obj);
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return $"{DomainName,-32}\t{QuestionClass}\t{QuestionType}";
        }

        /// <summary> Gets the bytes in this collection. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process the bytes in this collection.
        /// </returns>
        public IEnumerable<byte> GetBytes()
        {
            var data = new List<byte>();
            data.AddRange(WriteName(DomainName));
            data.AddRange(((ushort)QuestionType).GetBytes());
            data.AddRange(((ushort)QuestionClass).GetBytes());
            return data.ToArray();
        }

        /// <summary> Serves as the default hash function. </summary>
        /// <remarks> Dieter (coder2000) Lunn, 2020-04-01. </remarks>
        /// <returns> A hash code for the current object. </returns>
        /// <seealso cref="object.GetHashCode()"/>
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

            sb[^1] = '\0';
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
    }
}
