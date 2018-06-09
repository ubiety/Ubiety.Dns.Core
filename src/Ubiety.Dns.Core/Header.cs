using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS Record header
    /// </summary>
    public class Header
    {
        // internal flag
        private UInt16 flags;

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
            this.Id = rr.ReadUInt16();
            this.flags = rr.ReadUInt16();
            this.QuestionCount = rr.ReadUInt16();
            this.AnswerCount = rr.ReadUInt16();
            this.NameserverCount = rr.ReadUInt16();
            this.AdditionalRecordsCount = rr.ReadUInt16();
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
            get => GetBits(this.flags, 15, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 15, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets the record opcode flag
        /// </summary>
        public OperationCode OpCode
        {
            get => (OperationCode)GetBits(this.flags, 11, 4);
            set
            {
                this.flags = SetBits(this.flags, 11, 4, (UInt16)value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the record is an authoritative answer
        /// </summary>
        public Boolean AuthoritativAnswer
        {
            get => GetBits(this.flags, 10, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 10, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the truncation flag is set
        /// </summary>
        public Boolean Truncation
        {
            get => GetBits(this.flags, 9, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 9, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion flag is set
        /// </summary>
        public Boolean Recursion
        {
            get => GetBits(this.flags, 8, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 8, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the recursion available flag is set
        /// </summary>
        public Boolean RA
        {
            get => GetBits(this.flags, 7, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 7, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets a record reserved flag
        /// </summary>
        public UInt16 Z
        {
            get => GetBits(this.flags, 4, 3);
            set
            {
                this.flags = SetBits(this.flags, 4, 3, value);
            }
        }

        /// <summary>
        ///     Gets or sets the record response code
        /// </summary>
        public ResponseCode ResponseCode
        {
            get => (ResponseCode)GetBits(this.flags, 0, 4);
            set
            {
                this.flags = SetBits(this.flags, 0, 4, (UInt16)value);
            }
        }

        /// <summary>
        ///     Gets the header as a byte array
        /// </summary>
        /// <returns>Byte array of the header data</returns>
        public Byte[] GetData()
        {
            List<Byte> data = new List<Byte>();
            data.AddRange(WriteShort(this.Id));
            data.AddRange(WriteShort(this.flags));
            data.AddRange(WriteShort(this.QuestionCount));
            data.AddRange(WriteShort(this.AnswerCount));
            data.AddRange(WriteShort(this.NameserverCount));
            data.AddRange(WriteShort(this.AdditionalRecordsCount));
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
            Int32 mask = (2 << (length - 1)) - 1;

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
            Int32 mask = (2 << (length - 1)) - 1;

            // shift down to get some value and mask it
            return (UInt16)((oldValue >> position) & mask);
        }

        private static Byte[] WriteShort(UInt16 sValue)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((Int16)sValue));
        }
    }
}
