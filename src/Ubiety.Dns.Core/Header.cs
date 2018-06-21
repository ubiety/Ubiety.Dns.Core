using System;
using System.Collections.Generic;
using System.Net;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS Record header
    /// </summary>
    public class Header
    {
        // internal flag
        private UInt16 _flags;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Header" /> class
        /// </summary>
        public Header()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Header" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> of the record</param>
        public Header(RecordReader rr)
        {
            Id = rr.ReadUInt16();
            _flags = rr.ReadUInt16();
            QuestionCount = rr.ReadUInt16();
            AnswerCount = rr.ReadUInt16();
            NameserverCount = rr.ReadUInt16();
            AdditionalRecordsCount = rr.ReadUInt16();
        }

        /// <summary>
        ///     Gets or sets the unique identifier of the record
        /// </summary>
        public UInt16 Id { get; set; }

        /// <summary>
        ///     Gets or sets the number of questions in the record
        /// </summary>
        public UInt16 QuestionCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of answers in the record
        /// </summary>
        public UInt16 AnswerCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of name servers in the record
        /// </summary>
        public UInt16 NameserverCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of additional records in the record
        /// </summary>
        public UInt16 AdditionalRecordsCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the record is a query or response
        /// </summary>
        public Boolean QueryResponse
        {
            get => GetBits(_flags, 15, 1) == 1;
            set => _flags = SetBits(_flags, 15, 1, value);
        }

        /// <summary>
        ///     Gets or sets the record opcode flag
        /// </summary>
        public OperationCode OpCode
        {
            get => (OperationCode)GetBits(_flags, 11, 4);
            set => _flags = SetBits(_flags, 11, 4, (UInt16)value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the record is an authoritative answer
        /// </summary>
        public Boolean AuthoritativeAnswer
        {
            get => GetBits(_flags, 10, 1) == 1;
            set => _flags = SetBits(_flags, 10, 1, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the truncation flag is set
        /// </summary>
        public Boolean Truncation
        {
            get => GetBits(_flags, 9, 1) == 1;
            set => _flags = SetBits(_flags, 9, 1, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion flag is set
        /// </summary>
        public Boolean Recursion
        {
            get => GetBits(_flags, 8, 1) == 1;
            set => _flags = SetBits(_flags, 8, 1, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion available flag is set
        /// </summary>
        public Boolean RA
        {
            get => GetBits(_flags, 7, 1) == 1;
            set => _flags = SetBits(_flags, 7, 1, value);
        }

        /// <summary>
        ///     Gets or sets a record reserved flag
        /// </summary>
        public UInt16 Z
        {
            get => GetBits(_flags, 4, 3);
            set => _flags = SetBits(_flags, 4, 3, value);
        }

        /// <summary>
        ///     Gets or sets the record response code
        /// </summary>
        public ResponseCode ResponseCode
        {
            get => (ResponseCode)GetBits(_flags, 0, 4);
            set => _flags = SetBits(_flags, 0, 4, (UInt16)value);
        }

        /// <summary>
        ///     Gets the header as a byte array
        /// </summary>
        /// <returns>Byte array of the header data</returns>
        public IEnumerable<Byte> GetData()
        {
            var data = new List<Byte>();
            data.AddRange(WriteShort(Id));
            data.AddRange(WriteShort(_flags));
            data.AddRange(WriteShort(QuestionCount));
            data.AddRange(WriteShort(AnswerCount));
            data.AddRange(WriteShort(NameserverCount));
            data.AddRange(WriteShort(AdditionalRecordsCount));
            return data.ToArray();
        }

        private static UInt16 SetBits(UInt16 oldValue, Int32 position, Int32 length, Boolean blnValue)
        {
            return SetBits(oldValue, position, length, blnValue ? (UInt16)1 : (UInt16)0);
        }

        private static UInt16 SetBits(UInt16 oldValue, Int32 position, Int32 length, UInt16 newValue)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return oldValue;
            }

            // get some mask to put on
            var mask = (2 << (length - 1)) - 1;

            // clear out value
            oldValue &= (UInt16)~(mask << position);

            // set new value
            oldValue |= (UInt16)((newValue & mask) << position);
            return oldValue;
        }

        private static UInt16 GetBits(UInt16 oldValue, Int32 position, Int32 length)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return 0;
            }

            // get some mask to put on
            var mask = (2 << (length - 1)) - 1;

            // shift down to get some value and mask it
            return (UInt16)((oldValue >> position) & mask);
        }

        private static IEnumerable<Byte> WriteShort(UInt16 value)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)value));
        }
    }
}