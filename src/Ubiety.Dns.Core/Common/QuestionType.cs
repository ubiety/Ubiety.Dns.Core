using System;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     DNS question type
    /// </summary>
    public enum QuestionType : Int32
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
        ///     Mail destination type (Obsolete - Use MX)
        /// </summary>
        [Obsolete]
        MD = RecordType.MD,

        /// <summary>
        ///     Mail forwarder type (Obsolete - use MX)
        /// </summary>
        [Obsolete]
        MF = RecordType.MF,

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
        PTR = RecordType.PTR,

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
        /// </summary>
        AFSDB = RecordType.AFSDB,    // AFS Data Base location
        /// <summary>
        /// </summary>
        X25 = RecordType.X25,        // X.25 address rfc1183
        /// <summary>
        /// </summary>
        ISDN = RecordType.ISDN,    // ISDN address rfc1183
        /// <summary>
        /// </summary>
        RT = RecordType.RT,        // The Route Through rfc1183

        /// <summary>
        /// </summary>
        NSAP = RecordType.NSAP,    // Network service access point address rfc1706
        /// <summary>
        /// </summary>
        NSAP_PTR = RecordType.NSAPPTR, // Obsolete, rfc1348

        /// <summary>
        /// </summary>
        SIG = RecordType.SIG,        // Cryptographic public key signature rfc2931 / rfc2535
        /// <summary>
        /// </summary>
        KEY = RecordType.KEY,        // Public key as used in DNSSEC rfc2535

        /// <summary>
        /// </summary>
        PX = RecordType.PX,        // Pointer to X.400/RFC822 mail mapping information rfc2163

        /// <summary>
        /// </summary>
        GPOS = RecordType.GPOS,    // Geographical position rfc1712 (obsolete)

        /// <summary>
        /// </summary>
        AAAA = RecordType.AAAA,    // a IPV6 host address

        /// <summary>
        /// </summary>
        LOC = RecordType.LOC,        // Location information rfc1876

        /// <summary>
        /// </summary>
        NXT = RecordType.NXT,        // Obsolete rfc2065 / rfc2535

        /// <summary>
        /// </summary>
        EID = RecordType.EID,        // *** Endpoint Identifier (Patton)
        /// <summary>
        /// </summary>
        NIMLOC = RecordType.NIMLOC, // *** Nimrod Locator (Patton)

        /// <summary>
        /// </summary>
        SRV = RecordType.SRV,        // Location of services rfc2782

        /// <summary>
        /// </summary>
        ATMA = RecordType.ATMA,    // *** ATM Address (Dobrowski)

        /// <summary>
        /// </summary>
        NAPTR = RecordType.NAPTR,    // The Naming Authority Pointer rfc3403

        /// <summary>
        /// </summary>
        KX = RecordType.KX,        // Key Exchange Delegation Record rfc2230

        /// <summary>
        /// </summary>
        CERT = RecordType.CERT,    // *** CERT RFC2538

        /// <summary>
        /// </summary>
        A6 = RecordType.A6,        // IPv6 address rfc3363
        /// <summary>
        /// </summary>
        DNAME = RecordType.DNAME,    // A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672

        /// <summary>
        /// </summary>
        SINK = RecordType.SINK,    // *** SINK Eastlake
        /// <summary>
        /// </summary>
        OPT = RecordType.OPT,        // *** OPT RFC2671

        /// <summary>
        /// </summary>
        APL = RecordType.APL,        // *** APL [RFC3123]

        /// <summary>
        /// </summary>
        DS = RecordType.DS,        // Delegation Signer rfc3658

        /// <summary>
        /// </summary>
        SSHFP = RecordType.SSHFP,    // *** SSH Key Fingerprint RFC-ietf-secsh-dns
        /// <summary>
        /// </summary>
        IPSECKEY = RecordType.IPSECKEY, // rfc4025
        /// <summary>
        /// </summary>
        RRSIG = RecordType.RRSIG,    // *** RRSIG RFC-ietf-dnsext-dnssec-2535
        /// <summary>
        /// </summary>
        NSEC = RecordType.NSEC,    // *** NSEC RFC-ietf-dnsext-dnssec-2535
        /// <summary>
        /// </summary>
        DNSKEY = RecordType.DNSKEY,// *** DNSKEY RFC-ietf-dnsext-dnssec-2535
        /// <summary>
        /// </summary>
        DHCID = RecordType.DHCID,    // rfc4701

        /// <summary>
        /// </summary>
        NSEC3 = RecordType.NSEC3,    // RFC5155
        /// <summary>
        /// </summary>
        NSEC3PARAM = RecordType.NSEC3PARAM, // RFC5155

        /// <summary>
        /// </summary>
        HIP = RecordType.HIP,        // RFC-ietf-hip-dns-09.txt

        /// <summary>
        /// </summary>
        SPF = RecordType.SPF,        // RFC4408
        /// <summary>
        /// </summary>
        UINFO = RecordType.UINFO,    // *** IANA-Reserved
        /// <summary>
        /// </summary>
        UID = RecordType.UID,        // *** IANA-Reserved
        /// <summary>
        /// </summary>
        GID = RecordType.GID,        // *** IANA-Reserved
        /// <summary>
        /// </summary>
        UNSPEC = RecordType.UNSPEC,// *** IANA-Reserved

        /// <summary>
        /// </summary>
        TKEY = RecordType.TKEY,    // Transaction key rfc2930
        /// <summary>
        /// </summary>
        TSIG = RecordType.TSIG,    // Transaction signature rfc2845

        /// <summary>
        /// </summary>
        IXFR = 251,            // incremental transfer                  [RFC1995]
        /// <summary>
        /// </summary>
        AXFR = 252,            // transfer of an entire zone            [RFC1035]
        /// <summary>
        /// </summary>
        MAILB = 253,        // mailbox-related RRs (MB, MG or MR)    [RFC1035]
        /// <summary>
        /// </summary>
        MAILA = 254,        // mail agent RRs (Obsolete - see MX)    [RFC1035]
        /// <summary>
        /// </summary>
        ANY = 255,            // A request for all records             [RFC1035]

        /// <summary>
        /// </summary>
        TA = RecordType.TA,        // DNSSEC Trust Authorities    [Weiler]  13 December 2005
        /// <summary>
        /// </summary>
        DLV = RecordType.DLV        // DNSSEC Lookaside Validation [RFC4431]
    }
}