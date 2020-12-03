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

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS record reader.
    /// </summary>
    public class RecordReader
    {
        private readonly byte[] _data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordReader" /> class.
        /// </summary>
        /// <param name="data">Byte array of the record.</param>
        /// <param name="position">Position of the cursor in the record, defaults to 0.</param>
        public RecordReader(byte[] data, int position = 0)
        {
            _data = data;
            Position = position;
        }

        /// <summary>
        ///     Gets or sets the position of the cursor in the record.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        ///     Read a byte from the record.
        /// </summary>
        /// <returns>Next available byte of the record.</returns>
        public byte ReadByte()
        {
            return Position >= _data.Length ? (byte)0 : _data[Position++];
        }

        /// <summary>
        ///     Read a char from the record.
        /// </summary>
        /// <returns>Next available char of the record.</returns>
        public char ReadChar()
        {
            return (char)ReadByte();
        }

        /// <summary>
        ///     Read an unsigned int 16 from the record.
        /// </summary>
        /// <returns>Next available unsigned int 16 of the record.</returns>
        public ushort ReadUInt16()
        {
            return (ushort)((ReadByte() << 8) | ReadByte());
        }

        /// <summary>
        ///     Read an unsigned int 16 from the offset of the record.
        /// </summary>
        /// <param name="offset">Offset to start reading from.</param>
        /// <returns>Next unsigned int 16 from the offset.</returns>
        public ushort ReadUInt16(int offset)
        {
            Position += offset;
            return ReadUInt16();
        }

        /// <summary>
        ///     Read an unsigned int 32 from the record.
        /// </summary>
        /// <returns>Next available unsigned int 32 in the record.</returns>
        public uint ReadUInt32()
        {
            return (uint)((ReadUInt16() << 16) | ReadUInt16());
        }

        /// <summary>
        ///     Read the domain name from the record.
        /// </summary>
        /// <returns>Domain name of the record.</returns>
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
        ///     Read a string from the record.
        /// </summary>
        /// <returns>String read from the record.</returns>
        public string ReadString()
        {
            short length = ReadByte();

            return Encoding.UTF8.GetString(ReadBytes(length));
        }

        /// <summary>
        ///     Read a series of bytes from the record.
        /// </summary>
        /// <param name="length">Length to read from the record.</param>
        /// <returns>Byte array read from the record.</returns>
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
        ///     Read record from the data.
        /// </summary>
        /// <param name="type">Type of the record to read.</param>
        /// <returns>Record read from the data.</returns>
        public Record ReadRecord(RecordType type)
        {
            return type.GetRecord(this);
        }
    }
}
