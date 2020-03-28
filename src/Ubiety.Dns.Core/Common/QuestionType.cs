/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     DNS question type.
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        ///     IPv4 host address type
        /// </summary>
        A = RecordType.A,

        /// <summary>
        ///     An authoritative nameserver
        /// </summary>
        NS = RecordType.NS,

        /// <summary>
        ///     Canonical name DNS record type
        /// </summary>
        CNAME = RecordType.CNAME,

        /// <summary>
        ///     Marks the start of a zone of authority
        /// </summary>
        SOA = RecordType.SOA,

        /// <summary>
        ///     Mailbox domain name type (EXPERIMENTAL)
        /// </summary>
        MB = RecordType.MB,

        /// <summary>
        ///     Mail group member type (EXPERIMENTAL)
        /// </summary>
        MG = RecordType.MG,

        /// <summary>
        ///     Mail rename domain type (EXPERIMENTAL)
        /// </summary>
        MR = RecordType.MR,

        /// <summary>
        ///     Null resource record type (EXPERIMENTAL)
        /// </summary>
        NULL = RecordType.NULL,

        /// <summary>
        ///     Well known service description type
        /// </summary>
        WKS = RecordType.WKS,

        /// <summary>
        ///     Domain name pointer type
        /// </summary>
        PNTR = RecordType.PNTR,

        /// <summary>
        ///     Host information type
        /// </summary>
        HINFO = RecordType.HINFO,

        /// <summary>
        ///     Mailbox or mail list information
        /// </summary>
        MINFO = RecordType.MINFO,

        /// <summary>
        ///     Mail exchange type
        /// </summary>
        MX = RecordType.MX,

        /// <summary>
        ///     Text string type
        /// </summary>
        TXT = RecordType.TXT,

        /// <summary>
        ///     Responsible person DNS type
        /// </summary>
        RP = RecordType.RP,

        /// <summary>
        ///     AFS database location
        /// </summary>
        AFSDB = RecordType.AFSDB,

        /// <summary>
        ///     X.25 address
        /// </summary>
        X25 = RecordType.X25,

        /// <summary>
        ///     ISDN address
        /// </summary>
        ISDN = RecordType.ISDN,

        /// <summary>
        ///     Route through DNS type
        /// </summary>
        RT = RecordType.RT,

        /// <summary>
        ///     Network service access point address
        /// </summary>
        NSAP = RecordType.NSAP,

        /// <summary>
        ///     Cryptographic public key signature
        /// </summary>
        SIG = RecordType.SIG,

        /// <summary>
        ///     Public key for DNSSEC
        /// </summary>
        KEY = RecordType.KEY,

        /// <summary>
        ///     Pointer to X.400 mail mapping information
        /// </summary>
        PX = RecordType.PX,

        /// <summary>
        ///     IPv6 address DNS type
        /// </summary>
        AAAA = RecordType.AAAA,

        /// <summary>
        ///     DNS location information
        /// </summary>
        LOC = RecordType.LOC,

        /// <summary>
        ///     Location of services
        /// </summary>
        SRV = RecordType.SRV,

        /// <summary>
        ///     Naming authority pointer
        /// </summary>
        NAPTR = RecordType.NAPTR,

        /// <summary>
        ///     Key exchange delegation record
        /// </summary>
        KX = RecordType.KX,

        /// <summary>
        ///     Certificate DNS record
        /// </summary>
        CERT = RecordType.CERT,

        /// <summary>
        ///     Delegation signer DNS type
        /// </summary>
        DS = RecordType.DS,

        /// <summary>
        ///     Transaction key DNS type
        /// </summary>
        TKEY = RecordType.TKEY,

        /// <summary>
        ///     Transaction signature DNS type
        /// </summary>
        TSIG = RecordType.TSIG,

        /// <summary>
        ///     Incremental transfer
        /// </summary>
        IXFR = 251,

        /// <summary>
        ///     Zone transfer question
        /// </summary>
        AXFR = 252,

        /// <summary>
        ///     Mailbox resource record question
        /// </summary>
        MAILB = 253,

        /// <summary>
        ///     Mail agent resource record question
        /// </summary>
        MAILA = 254,

        /// <summary>
        ///     Request all records for a domain
        /// </summary>
        ANY = 255,
    }
}