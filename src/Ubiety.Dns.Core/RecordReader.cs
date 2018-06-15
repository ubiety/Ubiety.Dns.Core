using System;
using System.Collections.Generic;
using System.Text;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Dns.Core.Records.NotUsed;
using Ubiety.Dns.Core.Records.Obsolete;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     DNS record reader
    /// </summary>
    public class RecordReader
    {
        private readonly Byte[] data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordReader" /> class
        /// </summary>
        /// <param name="data">Byte array of the record</param>
        public RecordReader(Byte[] data)
        {
            this.data = data;
            this.Position = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordReader" /> class
        /// </summary>
        /// <param name="data">Byte array of the record</param>
        /// <param name="position">Position of the cursor in the record</param>
        public RecordReader(Byte[] data, Int32 position)
        {
            this.data = data;
            this.Position = position;
        }

        /// <summary>
        ///     Gets or sets the position of the cursor in the record
        /// </summary>
        public Int32 Position { get; set; }

        /// <summary>
        ///     Read a byte from the record
        /// </summary>
        /// <returns>Next available byte of the record</returns>
        public Byte ReadByte()
        {
            if (this.Position >= this.data.Length)
            {
                return 0;
            }
            else
            {
                return this.data[this.Position++];
            }
        }

        /// <summary>
        ///     Read a char from the record
        /// </summary>
        /// <returns>Next available char of the record</returns>
        public Char ReadChar()
        {
            return (char)this.ReadByte();
        }

        /// <summary>
        ///     Read an unsigned int 16 from the record
        /// </summary>
        /// <returns>Next available unsigned int 16 of the record</returns>
        public UInt16 ReadUInt16()
        {
            return (UInt16)(this.ReadByte() << 8 | this.ReadByte());
        }

        /// <summary>
        ///     Read an unsigned int 16 from the offset of the record
        /// </summary>
        /// <param name="offset">Offset to start reading from</param>
        /// <returns>Next unsigned int 16 from the offset</returns>
        public UInt16 ReadUInt16(Int32 offset)
        {
            this.Position += offset;
            return this.ReadUInt16();
        }

        /// <summary>
        ///     Read an unsigned int 32 from the record
        /// </summary>
        /// <returns>Next available unsigned int 32 in the record</returns>
        public UInt32 ReadUInt32()
        {
            return (UInt32)(this.ReadUInt16() << 16 | this.ReadUInt16());
        }

        /// <summary>
        ///     Read the domain name from the record
        /// </summary>
        /// <returns>Domain name of the record</returns>
        public String ReadDomainName()
        {
            StringBuilder name = new StringBuilder();
            Int32 length = 0;

            // get  the length of the first label
            while ((length = this.ReadByte()) != 0)
            {
                // top 2 bits set denotes domain name compression and to reference elsewhere
                if ((length & 0xc0) == 0xc0)
                {
                    // work out the existing domain name, copy this pointer
                    RecordReader newRecordReader = new RecordReader(this.data, (length & 0x3f) << 8 | this.ReadByte());

                    name.Append(newRecordReader.ReadDomainName());
                    return name.ToString();
                }

                // if not using compression, copy a char at a time to the domain name
                while (length > 0)
                {
                    name.Append(this.ReadChar());
                    length--;
                }

                name.Append('.');
            }

            if (name.Length == 0)
            {
                return ".";
            }
            else
            {
                return name.ToString();
            }
        }

        /// <summary>
        ///     Read a string from the record
        /// </summary>
        /// <returns>String read from the record</returns>
        public String ReadString()
        {
            Int16 length = this.ReadByte();

            StringBuilder name = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                name.Append(this.ReadChar());
            }

            return name.ToString();
        }

        /// <summary>
        ///     Read a series of bytes from the record
        /// </summary>
        /// <param name="length">Length to read from the record</param>
        /// <returns>Byte array read from the record</returns>
        public Byte[] ReadBytes(Int32 length)
        {
            List<Byte> list = new List<Byte>();
            for (Int32 i = 0; i < length; i++)
            {
                list.Add(this.ReadByte());
            }

            return list.ToArray();
        }

        /// <summary>
        ///     Read record from the data
        /// </summary>
        /// <param name="type">Type of the record to read</param>
        /// <returns>Record read from the data</returns>
        public Record ReadRecord(RecordType type)
        {
            switch (type)
            {
                case RecordType.A:
                    return new RecordA(this);
                case RecordType.NS:
                    return new RecordNs(this);
                case RecordType.MD:
                    return new RecordMd(this);
                case RecordType.MF:
                    return new RecordMf(this);
                case RecordType.CNAME:
                    return new RecordCname(this);
                case RecordType.SOA:
                    return new RecordSoa(this);
                case RecordType.MB:
                    return new RecordMb(this);
                case RecordType.MG:
                    return new RecordMg(this);
                case RecordType.MR:
                    return new RecordMr(this);
                case RecordType.NULL:
                    return new RecordNull(this);
                case RecordType.WKS:
                    return new RecordWks(this);
                case RecordType.PNTR:
                    return new RecordPtr(this);
                case RecordType.HINFO:
                    return new RecordHinfo(this);
                case RecordType.MINFO:
                    return new RecordMinfo(this);
                case RecordType.MX:
                    return new RecordMx(this);
                case RecordType.TXT:
                    return new RecordTxt(this);
                case RecordType.RP:
                    return new RecordRp(this);
                case RecordType.AFSDB:
                    return new RecordAfsdb(this);
                case RecordType.X25:
                    return new RecordX25(this);
                case RecordType.ISDN:
                    return new RecordIsdn(this);
                case RecordType.RT:
                    return new RecordRt(this);
                case RecordType.NSAP:
                    return new RecordNsap(this);
                case RecordType.NSAPPTR:
                    return new RecordNsapPtr(this);
                case RecordType.SIG:
                    return new RecordSig(this);
                case RecordType.KEY:
                    return new RecordKey(this);
                case RecordType.PX:
                    return new RecordPx(this);
                case RecordType.GPOS:
                    return new RecordGpos(this);
                case RecordType.AAAA:
                    return new RecordAaaa(this);
                case RecordType.LOC:
                    return new RecordLoc(this);
                case RecordType.NXT:
                    return new RecordNxt(this);
                case RecordType.EID:
                    return new RecordEid(this);
                case RecordType.NIMLOC:
                    return new RecordNimloc(this);
                case RecordType.SRV:
                    return new RecordSrv(this);
                case RecordType.ATMA:
                    return new RecordAtma(this);
                case RecordType.NAPTR:
                    return new RecordNaptr(this);
                case RecordType.KX:
                    return new RecordKx(this);
                case RecordType.CERT:
                    return new RecordCert(this);
                case RecordType.A6:
                    return new RecordA6(this);
                case RecordType.DNAME:
                    return new RecordDname(this);
                case RecordType.SINK:
                    return new RecordSink(this);
                case RecordType.OPT:
                    return new RecordOpt(this);
                case RecordType.APL:
                    return new RecordApl(this);
                case RecordType.DS:
                    return new RecordDs(this);
                case RecordType.SSHFP:
                    return new RecordSshfp(this);
                case RecordType.IPSECKEY:
                    return new RecordIpsecKey(this);
                case RecordType.RRSIG:
                    return new RecordRrsig(this);
                case RecordType.NSEC:
                    return new RecordNsec(this);
                case RecordType.DNSKEY:
                    return new RecordDnsKey(this);
                case RecordType.DHCID:
                    return new RecordDhcid(this);
                case RecordType.NSEC3:
                    return new RecordNsec3(this);
                case RecordType.NSEC3PARAM:
                    return new RecordNsec3Param(this);
                case RecordType.HIP:
                    return new RecordHip(this);
                case RecordType.SPF:
                    return new RecordSpf(this);
                case RecordType.UINFO:
                    return new RecordUinfo(this);
                case RecordType.UID:
                    return new RecordUid(this);
                case RecordType.GID:
                    return new RecordGid(this);
                case RecordType.UNSPEC:
                    return new RecordUnspec(this);
                case RecordType.TKEY:
                    return new RecordTkey(this);
                case RecordType.TSIG:
                    return new RecordTsig(this);
                default:
                    return new RecordUnknown(this);
            }
        }
    }
}
