namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    /// </summary>
    public enum RecordType : ushort
    {
        /// <summary>
        /// </summary>
        A = 1,                // a IPV4 host address
        /// <summary>
        /// </summary>
        NS = 2,                // an authoritative name server
        /// <summary>
        /// </summary>
        MD = 3,                // a mail destination (Obsolete - use MX)
        /// <summary>
        /// </summary>
        MF = 4,                // a mail forwarder (Obsolete - use MX)
        /// <summary>
        /// </summary>
        CNAME = 5,            // the canonical name for an alias
        /// <summary>
        /// </summary>
        SOA = 6,            // marks the start of a zone of authority
        /// <summary>
        /// </summary>
        MB = 7,                // a mailbox domain name (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        MG = 8,                // a mail group member (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        MR = 9,                // a mail rename domain name (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        NULL = 10,            // a null RR (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        WKS = 11,            // a well known service description
        /// <summary>
        /// </summary>
        PTR = 12,            // a domain name pointer
        /// <summary>
        /// </summary>
        HINFO = 13,            // host information
        /// <summary>
        /// </summary>
        MINFO = 14,            // mailbox or mail list information
        /// <summary>
        /// </summary>
        MX = 15,            // mail exchange
        /// <summary>
        /// </summary>
        TXT = 16,            // text strings
        /// <summary>
        /// </summary>
        RP = 17,            // The Responsible Person rfc1183
        /// <summary>
        /// </summary>
        AFSDB = 18,            // AFS Data Base location
        /// <summary>
        /// </summary>
        X25 = 19,            // X.25 address rfc1183
        /// <summary>
        /// </summary>
        ISDN = 20,            // ISDN address rfc1183 
        /// <summary>
        /// </summary>
        RT = 21,            // The Route Through rfc1183
        /// <summary>
        /// </summary>
        NSAP = 22,            // Network service access point address rfc1706
        /// <summary>
        /// </summary>
        NSAPPTR = 23,        // Obsolete, rfc1348
        /// <summary>
        /// </summary>
        SIG = 24,            // Cryptographic public key signature rfc2931 / rfc2535
        /// <summary>
        /// </summary>
        KEY = 25,            // Public key as used in DNSSEC rfc2535

        /// <summary>
        /// </summary>
        PX = 26,            // Pointer to X.400/RFC822 mail mapping information rfc2163

        /// <summary>
        /// </summary>
        GPOS = 27,            // Geographical position rfc1712 (obsolete)

        /// <summary>
        /// </summary>
        AAAA = 28,            // a IPV6 host address, rfc3596

        /// <summary>
        /// </summary>
        LOC = 29,            // Location information rfc1876

        /// <summary>
        /// </summary>
        NXT = 30,            // Next Domain, Obsolete rfc2065 / rfc2535

        /// <summary>
        /// </summary>
        EID = 31,            // *** Endpoint Identifier (Patton)
        /// <summary>
        /// </summary>
        NIMLOC = 32,        // *** Nimrod Locator (Patton)

        /// <summary>
        /// </summary>
        SRV = 33,            // Location of services rfc2782

        /// <summary>
        /// </summary>
        ATMA = 34,            // *** ATM Address (Dobrowski)

        /// <summary>
        /// </summary>
        NAPTR = 35,            // The Naming Authority Pointer rfc3403

        /// <summary>
        /// </summary>
        KX = 36,            // Key Exchange Delegation Record rfc2230

        /// <summary>
        /// </summary>
        CERT = 37,            // *** CERT RFC2538

        /// <summary>
        /// </summary>
        A6 = 38,            // IPv6 address rfc3363 (rfc2874 rfc3226)
        /// <summary>
        /// </summary>
        DNAME = 39,            // A way to provide aliases for a whole domain, not just a single domain name as with CNAME. rfc2672

        /// <summary>
        /// </summary>
        SINK = 40,            // *** SINK Eastlake
        /// <summary>
        /// </summary>
        OPT = 41,            // *** OPT RFC2671

        /// <summary>
        /// </summary>
        APL = 42,            // *** APL [RFC3123]

        /// <summary>
        /// </summary>
        DS = 43,            // Delegation Signer rfc3658

        /// <summary>
        /// </summary>
        SSHFP = 44,            // SSH Key Fingerprint rfc4255
        /// <summary>
        /// </summary>
        IPSECKEY = 45,        // IPSECKEY rfc4025
        /// <summary>
        /// </summary>
        RRSIG = 46,            // RRSIG rfc3755
        /// <summary>
        /// </summary>
        NSEC = 47,            // NSEC rfc3755
        /// <summary>
        /// </summary>
        DNSKEY = 48,        // DNSKEY 3755
        /// <summary>
        /// </summary>
        DHCID = 49,            // DHCID rfc4701

        /// <summary>
        /// </summary>
        NSEC3 = 50,            // NSEC3 rfc5155
        /// <summary>
        /// </summary>
        NSEC3PARAM = 51,    // NSEC3PARAM rfc5155

        /// <summary>
        /// </summary>
        HIP = 55,            // Host Identity Protocol  [RFC-ietf-hip-dns-09.txt]

        /// <summary>
        /// </summary>
        SPF = 99,            // SPF rfc4408

        /// <summary>
        /// </summary>
        UINFO = 100,        // *** IANA-Reserved
        /// <summary>
        /// </summary>
        UID = 101,            // *** IANA-Reserved
        /// <summary>
        /// </summary>
        GID = 102,            // *** IANA-Reserved
        /// <summary>
        /// </summary>
        UNSPEC = 103,        // *** IANA-Reserved

        /// <summary>
        /// </summary>
        TKEY = 249,            // Transaction key rfc2930
        /// <summary>
        /// </summary>
        TSIG = 250,            // Transaction signature rfc2845

        /// <summary>
        /// </summary>
        TA=32768,            // DNSSEC Trust Authorities          [Weiler]  13 December 2005
        /// <summary>
        /// </summary>
        DLV=32769            // DNSSEC Lookaside Validation       [RFC4431]
    }
}