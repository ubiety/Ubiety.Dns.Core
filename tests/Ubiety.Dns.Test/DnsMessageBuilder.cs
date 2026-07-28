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

using System.Collections.Generic;
using System.Linq;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Builds DNS messages in wire format so tests can feed the parser realistic bytes.
    /// </summary>
    internal static class DnsMessageBuilder
    {
        /// <summary>Response, recursion desired and available, no error.</summary>
        internal const ushort NoErrorFlags = 0x8180;

        /// <summary>Response with a SERVFAIL response code.</summary>
        internal const ushort ServFailFlags = 0x8182;

        /// <summary>
        /// Assembles a complete DNS message from a header, one question and a set of answers.
        /// </summary>
        internal static byte[] Message(
            ushort id,
            ushort flags,
            string questionName,
            QuestionType questionType,
            params byte[][] answers)
        {
            var bytes = new List<byte>();

            bytes.AddRange(UInt16(id));
            bytes.AddRange(UInt16(flags));
            bytes.AddRange(UInt16(1));                          // question count
            bytes.AddRange(UInt16((ushort)answers.Length));     // answer count
            bytes.AddRange(UInt16(0));                          // authority count
            bytes.AddRange(UInt16(0));                          // additional count

            bytes.AddRange(Name(questionName));
            bytes.AddRange(UInt16((ushort)questionType));
            bytes.AddRange(UInt16((ushort)QuestionClass.IN));

            foreach (var answer in answers)
            {
                bytes.AddRange(answer);
            }

            return [.. bytes];
        }

        /// <summary>
        /// Assembles a message carrying all four sections, so authority and additional records are
        /// exercised as well as answers.
        /// </summary>
        internal static byte[] MessageWithSections(
            string questionName,
            QuestionType questionType,
            byte[][] answers,
            byte[][] authorities,
            byte[][] additionals)
        {
            var bytes = new List<byte>();

            bytes.AddRange(UInt16(0x2222));
            bytes.AddRange(UInt16(NoErrorFlags));
            bytes.AddRange(UInt16(1));
            bytes.AddRange(UInt16((ushort)answers.Length));
            bytes.AddRange(UInt16((ushort)authorities.Length));
            bytes.AddRange(UInt16((ushort)additionals.Length));

            bytes.AddRange(Name(questionName));
            bytes.AddRange(UInt16((ushort)questionType));
            bytes.AddRange(UInt16((ushort)QuestionClass.IN));

            foreach (var record in answers.Concat(authorities).Concat(additionals))
            {
                bytes.AddRange(record);
            }

            return [.. bytes];
        }

        /// <summary>
        /// Builds an A record answer carrying the four supplied address octets.
        /// </summary>
        internal static byte[] ARecord(string name, uint ttl, params byte[] address)
        {
            return ResourceRecord(name, RecordType.A, ttl, address);
        }

        /// <summary>
        /// Builds a minimal but structurally valid SOA record answer.
        /// </summary>
        internal static byte[] SoaRecord(string name, uint serial)
        {
            var rdata = new List<byte>();
            rdata.AddRange(Name($"ns.{name}"));   // primary nameserver
            rdata.AddRange(Name($"admin.{name}")); // responsible mailbox
            rdata.AddRange(UInt32(serial));
            rdata.AddRange(UInt32(7200));   // refresh
            rdata.AddRange(UInt32(3600));   // retry
            rdata.AddRange(UInt32(604800)); // expire
            rdata.AddRange(UInt32(300));    // minimum

            return ResourceRecord(name, RecordType.SOA, 3600, [.. rdata]);
        }

        /// <summary>
        /// Assembles a message with no question section, as servers may send for the continuation
        /// messages of a zone transfer.
        /// </summary>
        internal static byte[] MessageWithoutQuestion(ushort id, ushort flags, params byte[][] answers)
        {
            var bytes = new List<byte>();

            bytes.AddRange(UInt16(id));
            bytes.AddRange(UInt16(flags));
            bytes.AddRange(UInt16(0));                          // question count
            bytes.AddRange(UInt16((ushort)answers.Length));     // answer count
            bytes.AddRange(UInt16(0));                          // authority count
            bytes.AddRange(UInt16(0));                          // additional count

            foreach (var answer in answers)
            {
                bytes.AddRange(answer);
            }

            return [.. bytes];
        }

        /// <summary>
        /// Prefixes a message with the two byte big-endian length used by DNS over TCP.
        /// </summary>
        internal static byte[] Framed(params byte[][] messages)
        {
            var bytes = new List<byte>();
            foreach (var message in messages)
            {
                bytes.AddRange(UInt16((ushort)message.Length));
                bytes.AddRange(message);
            }

            return [.. bytes];
        }

        /// <summary>
        /// Builds a resource record of any type around the supplied resource data.
        /// </summary>
        internal static byte[] Record(string name, RecordType type, uint ttl, params byte[] rdata)
        {
            return ResourceRecord(name, type, ttl, rdata);
        }

        /// <summary>
        /// Encodes a DNS character-string: a single length octet followed by that many bytes.
        /// </summary>
        internal static byte[] CharString(string value)
        {
            var bytes = new List<byte> { (byte)value.Length };
            bytes.AddRange(value.Select(c => (byte)c));
            return [.. bytes];
        }

        /// <summary>
        /// Concatenates resource data fragments into a single resource data block.
        /// </summary>
        internal static byte[] Data(params byte[][] parts) => [.. parts.SelectMany(p => p)];

        private static byte[] ResourceRecord(string name, RecordType type, uint ttl, byte[] rdata)
        {
            var bytes = new List<byte>();
            bytes.AddRange(Name(name));
            bytes.AddRange(UInt16((ushort)type));
            bytes.AddRange(UInt16((ushort)OperationClass.IN));
            bytes.AddRange(UInt32(ttl));
            bytes.AddRange(UInt16((ushort)rdata.Length));
            bytes.AddRange(rdata);
            return [.. bytes];
        }

        internal static byte[] Name(string name)
        {
            var bytes = new List<byte>();
            foreach (var label in name.TrimEnd('.').Split('.'))
            {
                bytes.Add((byte)label.Length);
                bytes.AddRange(label.Select(c => (byte)c));
            }

            bytes.Add(0);
            return [.. bytes];
        }

        internal static byte[] UInt16(ushort value) => [(byte)(value >> 8), (byte)(value & 0xFF)];

        internal static byte[] UInt32(uint value) =>
            [(byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)(value & 0xFF)];
    }
}
