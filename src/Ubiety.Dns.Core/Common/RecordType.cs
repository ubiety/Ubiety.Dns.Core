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
    /// Represents the type of DNS records.
    /// </summary>
    public enum RecordType
    {
        /// <summary>
        /// IPv4 address record.
        /// </summary>
        [Record(typeof(RecordA))]
        A = 1,

        /// <summary>
        /// Name Server record.
        /// </summary>
        [Record(typeof(RecordNs))]
        NS = 2,

        /// <summary>
        /// Canonical name record, used for aliasing one domain name to another.
        /// </summary>
        [Record(typeof(RecordCname))]
        CNAME = 5,

        /// <summary>
        /// Start of Authority record.
        /// </summary>
        [Record(typeof(RecordSoa))]
        SOA = 6,

        /// <summary>
        /// Mailbox domain name record.
        /// </summary>
        [Record(typeof(RecordMb))]
        MB = 7,

        /// <summary>
        /// Mail group record.
        /// </summary>
        [Record(typeof(RecordMg))]
        MG = 8,

        /// <summary>
        /// Mailbox rename record.
        /// Used to specify a mailbox that should be renamed or redirected.
        /// </summary>
        [Record(typeof(RecordMr))]
        MR = 9,

        /// <summary>
        /// A NULL record used for experimental purposes, typically containing no meaningful data.
        /// </summary>
        [Record(typeof(RecordNull))]
        NULL = 10,

        /// <summary>
        /// Well-known service description record.
        /// </summary>
        [Record(typeof(RecordWks))]
        WKS = 11,

        /// <summary>
        /// Pointer record, commonly used to map an IP address to a hostname in reverse DNS lookups.
        /// </summary>
        [Record(typeof(RecordPtr))]
        PNTR = 12,

        /// <summary>
        /// Host information record.
        /// </summary>
        [Record(typeof(RecordHinfo))]
        HINFO = 13,

        /// <summary>
        /// MINFO (Mailbox Information) record.
        /// Used to specify mailbox or mail list information.
        /// </summary>
        [Record(typeof(RecordMinfo))]
        MINFO = 14,

        /// <summary>
        /// Mail exchange record used to specify mail servers for the domain.
        /// </summary>
        [Record(typeof(RecordMx))]
        MX = 15,

        /// <summary>
        /// Text record, used to hold descriptive or arbitrary textual information.
        /// </summary>
        [Record(typeof(RecordTxt))]
        TXT = 16,

        /// <summary>
        /// Responsible Person record.
        /// </summary>
        [Record(typeof(RecordRp))]
        RP = 17,

        /// <summary>
        /// AFS database record.
        /// </summary>
        [Record(typeof(RecordAfsdb))]
        AFSDB = 18,

        /// <summary>
        /// X.25 address record.
        /// </summary>
        [Record(typeof(RecordX25))]
        X25 = 19,

        /// <summary>
        /// ISDN address record.
        /// </summary>
        [Record(typeof(RecordIsdn))]
        ISDN = 20,

        /// <summary>
        /// Route Through record, specifies intermediate hosts to route a message through.
        /// </summary>
        [Record(typeof(RecordRt))]
        RT = 21,

        /// <summary>
        /// NSAP address record.
        /// </summary>
        [Record(typeof(RecordNsap))]
        NSAP = 22,

        /// <summary>
        /// Signature record used in DNSSEC for digital signatures.
        /// </summary>
        [Record(typeof(RecordSig))]
        SIG = 24,

        /// <summary>
        /// Represents a DNS record type for cryptographic public keys.
        /// </summary>
        [Record(typeof(RecordKey))]
        KEY = 25,

        /// <summary>
        /// Delegation of mail agent records.
        /// </summary>
        [Record(typeof(RecordPx))]
        PX = 26,

        /// <summary>
        /// IPv6 address record.
        /// </summary>
        [Record(typeof(RecordAaaa))]
        AAAA = 28,

        /// <summary>
        /// Location record, used to represent geographical location information.
        /// </summary>
        [Record(typeof(RecordLoc))]
        LOC = 29,

        /// <summary>
        /// Service locator record, used to specify the location of a specific service within a domain.
        /// </summary>
        [Record(typeof(RecordSrv))]
        SRV = 33,

        /// <summary>
        /// Naming Authority Pointer record, used for dynamic delegation and service discovery.
        /// </summary>
        [Record(typeof(RecordNaptr))]
        NAPTR = 35,

        /// <summary>
        /// Key Exchange record.
        /// </summary>
        [Record(typeof(RecordKx))]
        KX = 36,

        /// <summary>
        /// Certificate record.
        /// </summary>
        [Record(typeof(RecordCert))]
        CERT = 37,

        /// <summary>
        /// DNAME record, used for redirection of a subtree of the DNS namespace to another domain.
        /// </summary>
        [Record(typeof(RecordDname))]
        DNAME = 39,

        /// <summary>
        /// Delegation Signer record type.
        /// </summary>
        [Record(typeof(RecordDs))]
        DS = 43,

        /// <summary>
        /// Transaction key record.
        /// </summary>
        [Record(typeof(RecordTkey))]
        TKEY = 249,

        /// <summary>
        /// Transaction Signature record used to provide authentication for DNS messages.
        /// </summary>
        [Record(typeof(RecordTsig))]
        TSIG = 250,
    }
}
