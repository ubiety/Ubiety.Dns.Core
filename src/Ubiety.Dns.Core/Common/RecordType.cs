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

using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Dns.Core.Records.Mail;

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
        ///     Location of services
        /// </summary>
        [Record(typeof(RecordSrv))]
        SRV = 33,

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
        ///     Domain alias
        /// </summary>
        [Record(typeof(RecordDname))]
        DNAME = 39,

        /// <summary>
        ///     Delegation signer
        /// </summary>
        [Record(typeof(RecordDs))]
        DS = 43,

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
    }
}
