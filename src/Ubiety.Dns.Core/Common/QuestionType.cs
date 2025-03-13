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

namespace Ubiety.Dns.Core.Common;

/// <summary>
/// Specifies the different types of questions that can be queried in a DNS protocol.
/// </summary>
public enum QuestionType
{
    /// <summary>
    /// A host address record.
    /// </summary>
    A = RecordType.A,

    /// <summary>
    /// A DNS name server record.
    /// </summary>
    NS = RecordType.NS,

    /// <summary>
    /// A canonical name record used to alias one domain name to another.
    /// </summary>
    CNAME = RecordType.CNAME,

    /// <summary>
    /// Specifies a Start of Authority (SOA) record, which contains important information about a DNS zone, including the primary name server, administrator's email, and zone properties.
    /// </summary>
    SOA = RecordType.SOA,

    /// <summary>
    /// A mailbox domain name record.
    /// </summary>
    MB = RecordType.MB,

    /// <summary>
    /// A mail group resource record.
    /// </summary>
    MG = RecordType.MG,

    /// <summary>
    /// A mail rename domain name.
    /// </summary>
    MR = RecordType.MR,

    /// <summary>
    /// Represents a NULL resource record.
    /// </summary>
    NULL = RecordType.NULL,

    /// <summary>
    /// A well-known service record.
    /// </summary>
    WKS = RecordType.WKS,

    /// <summary>
    /// A pointer record, used to map an IP address to a domain name in reverse DNS lookups.
    /// </summary>
    PNTR = RecordType.PNTR,

    /// <summary>
    /// Host information record, providing CPU and operating system details.
    /// </summary>
    HINFO = RecordType.HINFO,

    /// <summary>
    /// Represents a mailbox or mail group information record.
    /// </summary>
    MINFO = RecordType.MINFO,

    /// <summary>
    /// A mail exchange record.
    /// </summary>
    MX = RecordType.MX,

    /// <summary>
    /// A text record used to store descriptive or configuration information.
    /// </summary>
    TXT = RecordType.TXT,

    /// <summary>
    /// A resource record type for Responsible Person.
    /// </summary>
    RP = RecordType.RP,

    /// <summary>
    /// A record that specifies the location of an AFS cell database server.
    /// </summary>
    AFSDB = RecordType.AFSDB,

    /// <summary>
    /// An X.25 PSDN address record.
    /// </summary>
    X25 = RecordType.X25,

    /// <summary>
    /// Integrated Services Digital Network address record.
    /// </summary>
    ISDN = RecordType.ISDN,

    /// <summary>
    /// A route-through record.
    /// </summary>
    RT = RecordType.RT,

    /// <summary>
    /// Represents a Network Service Access Point (NSAP) address record.
    /// </summary>
    NSAP = RecordType.NSAP,

    /// <summary>
    /// A resource record type used for digital signatures in DNSSEC.
    /// </summary>
    SIG = RecordType.SIG,

    /// <summary>
    /// Represents a DNS security key record.
    /// </summary>
    KEY = RecordType.KEY,

    /// <summary>
    /// Pointer to X.400/RFC822 mapping information.
    /// </summary>
    PX = RecordType.PX,

    /// <summary>
    /// A record specifying an IPv6 address.
    /// </summary>
    AAAA = RecordType.AAAA,

    /// <summary>
    /// A location record.
    /// </summary>
    LOC = RecordType.LOC,

    /// <summary>
    /// A service locator record.
    /// </summary>
    SRV = RecordType.SRV,

    /// <summary>
    /// A Naming Authority Pointer record, used for service delegation in DNS.
    /// </summary>
    NAPTR = RecordType.NAPTR,

    /// <summary>
    /// A key exchange resource record.
    /// </summary>
    KX = RecordType.KX,

    /// <summary>
    /// A resource record used for storing certificates.
    /// </summary>
    CERT = RecordType.CERT,

    /// <summary>
    /// Delegation Signer record type.
    /// </summary>
    DS = RecordType.DS,

    /// <summary>
    /// Represents a Transaction Key record.
    /// </summary>
    TKEY = RecordType.TKEY,

    /// <summary>
    /// A transaction signature record used to provide authentication for DNS messages.
    /// </summary>
    TSIG = RecordType.TSIG,

    /// <summary>
    /// Represents an incremental zone transfer request.
    /// </summary>
    IXFR = 251,

    /// <summary>
    /// A request for a full zone transfer.
    /// </summary>
    AXFR = 252,

    /// <summary>
    /// A request for a mailbox-related record.
    /// </summary>
    MAILB = 253,

    /// <summary>
    /// A deprecated request for mail agent records.
    /// </summary>
    MAILA = 254,

    /// <summary>
    /// Represents a wildcard question type that matches any resource record.
    /// </summary>
    ANY = 255,
}