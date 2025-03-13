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
    /// <returns>The domain name of the record.</returns>
    public string ReadDomainName()
    {
        var name = new StringBuilder();
        int length;

        // get  the length of the first label
        while ((length = ReadByte()) != 0)
        {
            // top 2 bits set denotes domain name compression and to reference elsewhere
            if ((length & 0xc0) == 0xc0)
            {
                // work out the existing domain name, copy this pointer
                var newRecordReader = new RecordReader(_data, ((length & 0x3f) << 8) | ReadByte());

                name.Append(newRecordReader.ReadDomainName());
                return name.ToString();
            }

            // if not using compression, copy a char at a time to the domain name
            while (length > 0)
            {
                name.Append(ReadChar());
                length--;
            }

            name.Append('.');
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