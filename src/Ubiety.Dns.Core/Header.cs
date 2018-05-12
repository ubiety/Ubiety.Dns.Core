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
        /// An identifier assigned by the program
        /// </summary>
        public ushort Id;

        // internal flag
        private ushort Flags;

        /// <summary>
        /// the number of entries in the question section
        /// </summary>
        public ushort QuestionCount { get; set; }

        /// <summary>
        /// the number of resource records in the answer section
        /// </summary>
        public ushort AnswerCount { get; set; }

        /// <summary>
        /// the number of name server resource records in the authority records section
        /// </summary>
        public ushort NSCOUNT;

        /// <summary>
        /// the number of resource records in the additional records section
        /// </summary>
        public ushort ARCOUNT;

        /// <summary>
        /// </summary>
        public Header()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="rr"></param>
        public Header(RecordReader rr)
        {
            Id = rr.ReadUInt16();
            Flags = rr.ReadUInt16();
            this.QuestionCount = rr.ReadUInt16();
            this.AnswerCount = rr.ReadUInt16();
            NSCOUNT = rr.ReadUInt16();
            ARCOUNT = rr.ReadUInt16();
        }

        private ushort SetBits(ushort oldValue, int position, int length, bool blnValue)
        {
            return SetBits(oldValue, position, length, blnValue ? (ushort)1 : (ushort)0);
        }

        private ushort SetBits(ushort oldValue, int position, int length, ushort newValue)
        {
            // sanity check
            if (length <= 0 || position >= 16)
                return oldValue;

            // get some mask to put on
            int mask = (2 << (length - 1)) - 1;

            // clear out value
            oldValue &= (ushort)~(mask << position);

            // set new value
            oldValue |= (ushort)((newValue & mask) << position);
            return oldValue;
        }

        private ushort GetBits(ushort oldValue, int position, int length)
        {
            // sanity check
            if (length <= 0 || position >= 16)
                return 0;

            // get some mask to put on
            int mask = (2 << (length - 1)) - 1;

            // shift down to get some value and mask it
            return (ushort)((oldValue >> position) & mask);
        }

        /// <summary>
        /// Represents the header as a byte array
        /// </summary>
        public byte[] Data
        {
            get
            {
                List<byte> data = new List<byte>();
                data.AddRange(WriteShort(Id));
                data.AddRange(WriteShort(Flags));
                data.AddRange(WriteShort(this.QuestionCount));
                data.AddRange(WriteShort(this.AnswerCount));
                data.AddRange(WriteShort(NSCOUNT));
                data.AddRange(WriteShort(ARCOUNT));
                return data.ToArray();
            }
        }

        private byte[] WriteShort(ushort sValue)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)sValue));
        }

        /// <summary>
        /// query (false), or a response (true)
        /// </summary>
        public bool QR
        {
            get
            {
                return GetBits(Flags, 15, 1) == 1;
            }
            set
            {
                Flags = SetBits(Flags, 15, 1, value);
            }
        }

        /// <summary>
        /// Specifies kind of query
        /// </summary>
        public OPCode OPCODE
        {
            get
            {
                return (OPCode)GetBits(Flags, 11, 4);
            }
            set
            {
                Flags = SetBits(Flags, 11, 4, (ushort)value);
            }
        }

        /// <summary>
        /// Authoritative Answer
        /// </summary>
        public bool AA
        {
            get
            {
                return GetBits(Flags, 10, 1) == 1;
            }
            set
            {
                Flags = SetBits(Flags, 10, 1, value);
            }
        }

        /// <summary>
        /// TrunCation
        /// </summary>
        public bool TC
        {
            get
            {
                return GetBits(Flags, 9, 1) == 1;
            }
            set
            {
                Flags = SetBits(Flags, 9, 1, value);
            }
        }

        /// <summary>
        /// Recursion Desired
        /// </summary>
        public bool RD
        {
            get
            {
                return GetBits(Flags, 8, 1) == 1;
            }
            set
            {
                Flags = SetBits(Flags, 8, 1, value);
            }
        }

        /// <summary>
        /// Recursion Available
        /// </summary>
        public bool RA
        {
            get
            {
                return GetBits(Flags, 7, 1) == 1;
            }
            set
            {
                Flags = SetBits(Flags, 7, 1, value);
            }
        }

        /// <summary>
        /// Reserved for future use
        /// </summary>
        public ushort Z
        {
            get
            {
                return GetBits(Flags, 4, 3);
            }
            set
            {
                Flags = SetBits(Flags, 4, 3, value);
            }
        }

        /// <summary>
        /// Response code
        /// </summary>
        public RCode RCODE
        {
            get
            {
                return (RCode)GetBits(Flags, 0, 4);
            }
            set
            {
                Flags = SetBits(Flags, 0, 4, (ushort)value);
            }
        }
    }
}
