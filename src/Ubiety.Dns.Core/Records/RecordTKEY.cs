using System;
using System.Globalization;
/*
* http://tools.ietf.org/rfc/rfc2930.txt
* 
2. The TKEY Resource Record

The TKEY resource record (RR) has the structure given below.  Its RR
type code is 249.

Field       Type         Comment
-----       ----         -------
Algorithm:   domain
Inception:   u_int32_t
Expiration:  u_int32_t
Mode:        u_int16_t
Error:       u_int16_t
Key Size:    u_int16_t
Key Data:    octet-stream
Other Size:  u_int16_t
Other Data:  octet-stream  undefined by this specification

*/

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Transaction key DNS resource record
    /// </summary>
    public class RecordTKEY : Record
    {
        /// <summary>
        ///     Gets the key algorithm
        /// </summary>
        public string Algorithm { get; }

        /// <summary>
        ///     Gets the inception time of the key
        /// </summary>
        public UInt32 Inception { get; }

        /// <summary>
        ///     Gets the expiration time of the key
        /// </summary>
        public UInt32 Expiration { get; }

        /// <summary>
        ///     Gets the key agreement mode
        /// </summary>
        public UInt16 Mode { get; }

        /// <summary>
        ///     Gets the error code of the record
        /// </summary>
        public UInt16 Error { get; }

        /// <summary>
        ///     Gets the key size from the record
        /// </summary>
        public UInt16 KeySize { get; }

        /// <summary>
        ///     Gets the key data
        /// </summary>
        public byte[] KeyData { get; }

        /// <summary>
        ///     Gets the other size from the record (Future use)
        /// </summary>
        public UInt16 OtherSize { get; }

        /// <summary>
        ///     Gets the other data from the record (Future use)
        /// </summary>
        public byte[] OtherData { get; }

        /// <summary>
        /// </summary>
        public RecordTKEY(RecordReader rr)
        {
            this.Algorithm = rr.ReadDomainName();
            this.Inception = rr.ReadUInt32();
            this.Expiration = rr.ReadUInt32();
            this.Mode = rr.ReadUInt16();
            this.Error = rr.ReadUInt16();
            this.KeySize = rr.ReadUInt16();
            this.KeyData = rr.ReadBytes(this.KeySize);
            this.OtherSize = rr.ReadUInt16();
            this.OtherData = rr.ReadBytes(this.OtherSize);
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4}",
                this.Algorithm,
                this.Inception,
                this.Expiration,
                this.Mode,
                this.Error);
        }

    }
}
