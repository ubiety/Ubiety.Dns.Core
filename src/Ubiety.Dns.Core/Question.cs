/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
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
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core;

/// <summary>
/// Represents a DNS question section, which contains information about the domain name being queried,
/// the type of query, and the class of query.
/// </summary>
public sealed class Question : IEquatable<Question>
{
    /// <summary>
    /// The maximum length of a single label in octets, per RFC 1035 section 2.3.4.
    /// </summary>
    private const int MaxLabelLength = 63;

    /// <summary>
    /// The maximum length of an encoded domain name in octets, per RFC 1035 section 2.3.4.
    /// </summary>
    private const int MaxDomainNameLength = 255;

    /// <summary>
    /// Initializes a new instance of the <see cref="Question"/> class.
    /// </summary>
    /// <param name="domainName">The domain name to query.</param>
    /// <param name="questionType">The type of query being performed.</param>
    /// <param name="questionClass">The class of the query.</param>
    public Question(string domainName, QuestionType questionType, QuestionClass questionClass)
    {
        ArgumentNullException.ThrowIfNull(domainName);
        if (!domainName.EndsWith('.'))
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
    /// <param name="reader"><see cref="RecordReader" /> of the record.</param>
    internal Question(RecordReader reader)
    {
        DomainName = reader.ReadDomainName();
        QuestionType = (QuestionType)reader.ReadUInt16();
        QuestionClass = (QuestionClass)reader.ReadUInt16();
    }

    /// <summary>
    /// Gets the domain name associated with the DNS question.
    /// </summary>
    /// <value>The domain name being queried.</value>
    public string DomainName { get; }

    /// <summary>
    /// Gets the type of DNS record associated with the question.
    /// </summary>
    /// <value>The DNS record type being queried, such as A, MX, or TXT.</value>
    public QuestionType QuestionType { get; }

    /// <summary>
    /// Gets the query class for the DNS question, determining the protocol group in use.
    /// </summary>
    /// <value>The class of the DNS query, such as IN, CS, CH, HS, or Any.</value>
    public QuestionClass QuestionClass { get; }

    /// <summary>
    /// Checks whether two <see cref="Question"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare, which may be null.</param>
    /// <param name="right">The second instance to compare, which may be null.</param>
    /// <returns>True if the two instances are equal; otherwise, false.</returns>
    public static bool operator ==(Question? left, Question? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two specified <see cref="Question"/> objects are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare, which may be null.</param>
    /// <param name="right">The second instance to compare, which may be null.</param>
    /// <returns><c>true</c> if the two <see cref="Question"/> objects are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Question? left, Question? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Determines whether the current <see cref="Question"/> instance is equal to another <see cref="Question"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="Question"/> instance to compare with the current instance, which may be null.</param>
    /// <returns>
    /// true if the current <see cref="Question"/> instance is equal to the <paramref name="other"/> parameter; otherwise, false.
    /// </returns>
    public bool Equals([NotNullWhen(true)] Question? other)
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

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Question"/> instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance, which may be null.</param>
    /// <returns>
    /// true if the specified object is equal to the current <see cref="Question"/> instance; otherwise, false.
    /// </returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
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

    /// <summary>
    /// Converts the current <see cref="Question"/> instance to its string representation.
    /// </summary>
    /// <returns>A string that represents the current object, including the domain name, class, and type of the query.</returns>
    public override string ToString()
    {
        return $"{DomainName,-32}\t{QuestionClass}\t{QuestionType}";
    }

    /// <summary>
    /// Converts the question information into a sequence of bytes that can be used in DNS requests or responses.
    /// </summary>
    /// <returns>An enumerable collection of bytes representing the encoded DNS question fields.</returns>
    public IEnumerable<byte> GetBytes()
    {
        return [.. WriteName(DomainName), .. ((ushort)QuestionType).GetBytes(), .. ((ushort)QuestionClass).GetBytes()];
    }

    /// <summary>
    /// Generates a hash code for the current instance of the <see cref="Question"/> class.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(DomainName, QuestionClass, QuestionType);
    }

    /// <summary>
    /// Encodes a domain name into the length-prefixed label sequence used on the wire.
    /// </summary>
    /// <param name="src">The domain name to encode, with or without a trailing separator.</param>
    /// <returns>The encoded name, terminated by the zero-length root label.</returns>
    /// <exception cref="FormatException">
    /// The name contains an empty label, a label longer than <see cref="MaxLabelLength"/> octets, or
    /// encodes to more than <see cref="MaxDomainNameLength"/> octets.
    /// </exception>
    private static byte[] WriteName(string src)
    {
        if (!src.EndsWith('.'))
        {
            src += ".";
        }

        // The root is a bare terminator with no labels of its own.
        if (src == ".")
        {
            return new byte[1];
        }

        var bytes = new List<byte>(src.Length + 1);

        // The trailing separator terminates the name rather than introducing an empty label.
        foreach (var label in src[..^1].Split('.'))
        {
            if (label.Length == 0 || label.Length > MaxLabelLength)
            {
                throw new FormatException(
                    $"'{src}' contains a label that is empty or longer than {MaxLabelLength} octets.");
            }

            bytes.Add((byte)label.Length);
            bytes.AddRange(Encoding.ASCII.GetBytes(label));
        }

        // Zero-length root label terminating the name.
        bytes.Add(0);

        if (bytes.Count > MaxDomainNameLength)
        {
            throw new FormatException(
                $"'{src}' encodes to {bytes.Count} octets, exceeding the {MaxDomainNameLength} octet limit.");
        }

        return bytes.ToArray();
    }
}
