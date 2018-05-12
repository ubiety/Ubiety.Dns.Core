using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS Record header
    /// </summary>
    public class Header
    {
        /// <summary>
        ///     Gets or sets the unique identifier of the record
        /// </summary>
        public ushort Id { get; set; }

        // internal flag
        private ushort flags;

        /// <summary>
        ///     Gets or sets the number of questions in the record
        /// </summary>
        public ushort QuestionCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of answers in the record
        /// </summary>
        public ushort AnswerCount { get; set; }

        /// <summary>
        ///     Gets or sets the number of name servers in the record
        /// </summary>
        public ushort NameserverCount {get; set; }

        /// <summary>
        ///     Gets or sets the number of additional records in the record
        /// </summary>
        public ushort AdditionalRecordsCount { get; set; }

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

        private static ushort SetBits(ushort oldValue, int position, int length, bool blnValue)
        {
            return SetBits(oldValue, position, length, blnValue ? (ushort)1 : (ushort)0);
        }

        private static ushort SetBits(ushort oldValue, int position, int length, ushort newValue)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return oldValue;
            }

            // get some mask to put on
            int mask = (2 << (length - 1)) - 1;

            // clear out value
            oldValue &= (ushort)~(mask << position);

            // set new value
            oldValue |= (ushort)((newValue & mask) << position);
            return oldValue;
        }

        private static ushort GetBits(ushort oldValue, int position, int length)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return 0;
            }

            // get some mask to put on
            int mask = (2 << (length - 1)) - 1;

            // shift down to get some value and mask it
            return (ushort)((oldValue >> position) & mask);
        }

        /// <summary>
        ///     Gets the header as a byte array
        /// </summary>
        public byte[] Data
        {
            get
            {
                List<byte> data = new List<byte>();
                data.AddRange(WriteShort(this.Id));
                data.AddRange(WriteShort(this.flags));
                data.AddRange(WriteShort(this.QuestionCount));
                data.AddRange(WriteShort(this.AnswerCount));
                data.AddRange(WriteShort(this.NameserverCount));
                data.AddRange(WriteShort(this.AdditionalRecordsCount));
                return data.ToArray();
            }
        }

        private static byte[] WriteShort(ushort sValue)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)sValue));
        }

        /// <summary>
        ///     Gets or sets the query/response flag
        /// </summary>
        public bool QR
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
        public OPCode OPCODE
        {
            get => (OPCode)GetBits(this.flags, 11, 4);
            set
            {
                this.flags = SetBits(this.flags, 11, 4, (ushort)value);
            }
        }

        /// <summary>
        ///     Gets or sets the record authoritative answer flag
        /// </summary>
        public bool AA
        {
            get => GetBits(this.flags, 10, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 10, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets the record truncation flag
        /// </summary>
        public bool TC
        {
            get => GetBits(this.flags, 9, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 9, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets the record recursion desired flag
        /// </summary>
        public bool RD
        {
            get => GetBits(this.flags, 8, 1) == 1;
            set
            {
                this.flags = SetBits(this.flags, 8, 1, value);
            }
        }

        /// <summary>
        ///     Gets or sets the record recursion available flag
        /// </summary>
        public bool RA
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
        public ushort Z
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
        public RCode RCODE
        {
            get => (RCode)GetBits(this.flags, 0, 4);
            set
            {
                this.flags = SetBits(this.flags, 0, 4, (ushort)value);
            }
        }
    }
}
