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

using System.Collections.Generic;
using System.Text;

using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Common.Extensions;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Core;

/// <summary>
/// Provides utilities for reading DNS record data from a byte array.
/// </summary>
public class RecordReader(byte[] data, int position = 0)
{
    /// <summary>
    /// The maximum number of compression pointers followed while reading a single domain name.
    /// </summary>
    private const int MaxCompressionJumps = 128;

    /// <summary>
    /// The maximum length of a domain name in octets, per RFC 1035 section 2.3.4.
    /// </summary>
    private const int MaxDomainNameLength = 255;

    private readonly byte[] _data = data;

    /// <summary>
    /// Gets or sets the current reading position within the byte array.
    /// </summary>
    /// <remarks>
    /// The position represents the index in the byte array from which the next read operation will occur.
    /// Modifying this value directly affects subsequent read operations.
    /// </remarks>
    public int Position { get; set; } = position;

    /// <summary>
    /// Reads the next byte from the record.
    /// </summary>
    /// <returns>The next available byte of the record.</returns>
    public byte ReadByte()
    {
        return Position >= _data.Length ? (byte)0 : _data[Position++];
    }

    /// <summary>
    /// Reads the next character from the record.
    /// </summary>
    /// <returns>The next available character of the record.</returns>
    public char ReadChar()
    {
        return (char)ReadByte();
    }

    /// <summary>
    /// Reads the next unsigned 16-bit integer from the record.
    /// </summary>
    /// <returns>The next available unsigned 16-bit integer of the record.</returns>
    public ushort ReadUInt16()
    {
        return (ushort)((ReadByte() << 8) | ReadByte());
    }

    /// <summary>
    /// Reads the next unsigned 16-bit integer from the record.
    /// </summary>
    /// <param name="offset">Offset to start reading from.</param>
    /// <returns>The next available unsigned 16-bit integer of the record from the offset.</returns>
    public ushort ReadUInt16(int offset)
    {
        Position += offset;
        return ReadUInt16();
    }

    /// <summary>
    /// Reads the next unsigned 32-bit integer from the record.
    /// </summary>
    /// <returns>The next available unsigned 32-bit integer in the record.</returns>
    public uint ReadUInt32()
    {
        return (uint)((ReadUInt16() << 16) | ReadUInt16());
    }

    /// <summary>
    /// Reads and returns the domain name from the current record.
    /// </summary>
    /// <remarks>
    /// Compression pointers are followed iteratively and are bounded by
    /// <see cref="MaxCompressionJumps"/> jumps and <see cref="MaxDomainNameLength"/> octets, so a
    /// malformed or hostile response containing a pointer cycle terminates instead of looping.
    /// Reading stops early rather than throwing when either bound is reached.
    /// </remarks>
    /// <returns>The domain name of the record.</returns>
    public string ReadDomainName()
    {
        var name = new StringBuilder();
        var current = Position;
        var jumps = 0;
        var jumped = false;

        // get the length of each label in turn; a zero length terminates the name
        while (current < _data.Length)
        {
            int length = _data[current++];

            if (length == 0)
            {
                break;
            }

            // top 2 bits set denotes domain name compression and to reference elsewhere
            if ((length & 0xc0) == 0xc0)
            {
                if (current >= _data.Length)
                {
                    break;
                }

                var pointer = ((length & 0x3f) << 8) | _data[current++];

                // Only the pointer itself is consumed from the reader's own position; the
                // target is read out of band and must not advance the reader any further.
                if (!jumped)
                {
                    Position = current;
                    jumped = true;
                }

                if (++jumps > MaxCompressionJumps || pointer >= _data.Length)
                {
                    break;
                }

                current = pointer;
                continue;
            }

            // account for the separator appended after the label
            if (name.Length + length + 1 > MaxDomainNameLength)
            {
                break;
            }

            // if not using compression, copy a char at a time to the domain name
            while (length > 0 && current < _data.Length)
            {
                name.Append((char)_data[current++]);
                length--;
            }

            name.Append('.');
        }

        if (!jumped)
        {
            Position = current;
        }

        return name.Length == 0 ? "." : name.ToString();
    }

    /// <summary>
    /// Reads a string from the record using its length, which is determined by reading a byte preceding the string data.
    /// </summary>
    /// <returns>The string read from the record.</returns>
    public string ReadString()
    {
        short length = ReadByte();

        return Encoding.UTF8.GetString(ReadBytes(length));
    }

    /// <summary>
    /// Reads a sequence of bytes from the record.
    /// </summary>
    /// <param name="length">The number of bytes to read from the record.</param>
    /// <returns>An array containing the bytes read from the record.</returns>
    public byte[] ReadBytes(int length)
    {
        var list = new List<byte>();
        for (var i = 0; i < length; i++)
        {
            list.Add(ReadByte());
        }

        return list.ToArray();
    }

    /// <summary>
    /// Reads a record of the specified type from the data.
    /// </summary>
    /// <param name="type">The type of the record to be read.</param>
    /// <returns>The record read from the data.</returns>
    public Record ReadRecord(RecordType type)
    {
        return type.GetRecord(this);
    }
}