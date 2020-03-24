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

using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Dns.Core.Records.Mail;
using Ubiety.Dns.Core.Records.NotUsed;
using Ubiety.Dns.Core.Records.Obsolete;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     DNS record type.
    /// </summary>
    public enum RecordType
    {
        /// <summary>
        ///     IPv4 address
        /// </summary>
        [Record(typeof(RecordA))]
        A = 1,

        /// <summary>
        ///     Authoritative nameserver
        /// </summary>
        [Record(typeof(RecordNs))]
        NS = 2,

        /// <summary>
        ///     Mail destination
        /// </summary>
        [Record(typeof(RecordMd))]
        MD = 3,

        /// <summary>
        ///     Mail forwarder
        /// </summary>
        [Record(typeof(RecordMf))]
        MF = 4,

        /// <summary>
        ///     Canonical name for a domain alias
        /// </summary>
        [Record(typeof(RecordCname))]
        CNAME = 5,

        /// <summary>
        ///     The start of a zone of authority
        /// </summary>
        [Record(typeof(RecordSoa))]
        SOA = 6,

        /// <summary>
        ///     Mailbox domain name
        /// </summary>
        [Record(typeof(RecordMb))]
        MB = 7,

        /// <summary>
        ///     Mail group member
        /// </summary>
        [Record(typeof(RecordMg))]
        MG = 8,

        /// <summary>
        ///     Mail rename domain
        /// </summary>
        [Record(typeof(RecordMr))]
        MR = 9,

        /// <summary>
        ///     Null resource record
        /// </summary>
        [Record(typeof(RecordNull))]
        NULL = 10,

        /// <summary>
        ///     Well known service description
        /// </summary>
        [Record(typeof(RecordWks))]
        WKS = 11,

        /// <summary>
        ///     Domain pointer type
        /// </summary>
        [Record(typeof(RecordPtr))]
        PNTR = 12,

        /// <summary>
        ///     Host information type
        /// </summary>
        [Record(typeof(RecordHinfo))]
        HINFO = 13,

        /// <summary>
        ///     Mailbox or list information
        /// </summary>
        [Record(typeof(RecordMinfo))]
        MINFO = 14,

        /// <summary>
        ///     Mail exchange type
        /// </summary>
        [Record(typeof(RecordMx))]
        MX = 15,

        /// <summary>
        ///     Text string
        /// </summary>
        [Record(typeof(RecordTxt))]
        TXT = 16,

        /// <summary>
        ///     Responsible person type
        /// </summary>
        [Record(typeof(RecordRp))]
        RP = 17,

        /// <summary>
        ///     AFS database location
        /// </summary>
        [Record(typeof(RecordAfsdb))]
        AFSDB = 18,

        /// <summary>
        ///     X.25 address type
        /// </summary>
        [Record(typeof(RecordX25))]
        X25 = 19,

        /// <summary>
        ///     ISDN address type
        /// </summary>
        [Record(typeof(RecordIsdn))]
        ISDN = 20,

        /// <summary>
        ///     Route through DNS type
        /// </summary>
        [Record(typeof(RecordRt))]
        RT = 21,

        /// <summary>
        ///     Network service access point address
        /// </summary>
        [Record(typeof(RecordNsap))]
        NSAP = 22,

        /// <summary>
        ///     Obsolete DNS type
        /// </summary>
        [Record(typeof(RecordNsapPtr))]
        NSAPPTR = 23,

        /// <summary>
        ///     Cryptographic public key signature
        /// </summary>
        [Record(typeof(RecordSig))]
        SIG = 24,

        /// <summary>
        ///     DNSSEC public key
        /// </summary>
        [Record(typeof(RecordKey))]
        KEY = 25,

        /// <summary>
        ///     Pointer for X.400 mail mapping information
        /// </summary>
        [Record(typeof(RecordPx))]
        PX = 26,

        /// <summary>
        ///     Geographical position type
        /// </summary>
        [Record(typeof(RecordGpos))]
        GPOS = 27,

        /// <summary>
        ///     IPv6 host address
        /// </summary>
        [Record(typeof(RecordAaaa))]
        AAAA = 28,

        /// <summary>
        ///     Location information
        /// </summary>
        [Record(typeof(RecordLoc))]
        LOC = 29,

        /// <summary>
        ///     Next domain type
        /// </summary>
        [Record(typeof(RecordNxt))]
        NXT = 30,

        /// <summary>
        ///     Endpoint identifier
        /// </summary>
        [Record(typeof(RecordEid))]
        EID = 31,

        /// <summary>
        ///     Nimrod locator
        /// </summary>
        [Record(typeof(RecordNimloc))]
        NIMLOC = 32,

        /// <summary>
        ///     Location of services
        /// </summary>
        [Record(typeof(RecordSrv))]
        SRV = 33,

        /// <summary>
        ///     ATM address type
        /// </summary>
        [Record(typeof(RecordAtma))]
        ATMA = 34,

        /// <summary>
        ///     Naming authority pointer
        /// </summary>
        [Record(typeof(RecordNaptr))]
        NAPTR = 35,

        /// <summary>
        ///     Key exchange record type
        /// </summary>
        [Record(typeof(RecordKx))]
        KX = 36,

        /// <summary>
        ///     Certificate
        /// </summary>
        [Record(typeof(RecordCert))]
        CERT = 37,

        /// <summary>
        ///     IPv6 address (Historic)
        /// </summary>
        [Record(typeof(RecordA6))]
        A6 = 38,

        /// <summary>
        ///     Domain alias
        /// </summary>
        [Record(typeof(RecordDname))]
        DNAME = 39,

        /// <summary>
        ///     SINK type
        /// </summary>
        [Record(typeof(RecordSink))]
        SINK = 40,

        /// <summary>
        ///     OPT type
        /// </summary>
        [Record(typeof(RecordOpt))]
        OPT = 41,

        /// <summary>
        ///     APL type
        /// </summary>
        [Record(typeof(RecordApl))]
        APL = 42,

        /// <summary>
        ///     Delegation signer
        /// </summary>
        [Record(typeof(RecordDs))]
        DS = 43,

        /// <summary>
        ///     SSH key fingerprint
        /// </summary>
        [Record(typeof(RecordSshfp))]
        SSHFP = 44,

        /// <summary>
        ///     IPSEC key
        /// </summary>
        [Record(typeof(RecordIpsecKey))]
        IPSECKEY = 45,

        /// <summary>
        ///     Resource record signature
        /// </summary>
        [Record(typeof(RecordRrsig))]
        RRSIG = 46,

        /// <summary>
        ///     NSEC type
        /// </summary>
        [Record(typeof(RecordNsec))]
        NSEC = 47,

        /// <summary>
        ///     DNS key
        /// </summary>
        [Record(typeof(RecordDnsKey))]
        DNSKEY = 48,

        /// <summary>
        ///     DHCP information
        /// </summary>
        [Record(typeof(RecordDhcid))]
        DHCID = 49,

        /// <summary>
        ///     NSEC3 type
        /// </summary>
        [Record(typeof(RecordNsec3))]
        NSEC3 = 50,

        /// <summary>
        ///     NSEC3PARAM type
        /// </summary>
        [Record(typeof(RecordNsec3Param))]
        NSEC3PARAM = 51,

        /// <summary>
        ///     Host Identity protocol
        /// </summary>
        [Record(typeof(RecordHip))]
        HIP = 55,

        /// <summary>
        ///     SPF type
        /// </summary>
        [Record(typeof(RecordSpf))]
        SPF = 99,

        /// <summary>
        ///     Reserved
        /// </summary>
        [Record(typeof(RecordUinfo))]
        UINFO = 100,

        /// <summary>
        ///     Reserved
        /// </summary>
        [Record(typeof(RecordUid))]
        UID = 101,

        /// <summary>
        ///     Reserved
        /// </summary>
        [Record(typeof(RecordGid))]
        GID = 102,

        /// <summary>
        ///     Reserved
        /// </summary>
        [Record(typeof(RecordUnspec))]
        UNSPEC = 103,

        /// <summary>
        ///     Transaction key
        /// </summary>
        [Record(typeof(RecordTkey))]
        TKEY = 249,

        /// <summary>
        ///     Transaction signature
        /// </summary>
        [Record(typeof(RecordTsig))]
        TSIG = 250,

        /// <summary>
        ///     DNSSEC trust authorities
        /// </summary>
        [Record(typeof(RecordUnknown))]
        TA = 32768,

        /// <summary>
        ///     DNSSEC lookaside validation
        /// </summary>
        [Record(typeof(RecordUnknown))]
        DLV = 32769,
    }
}