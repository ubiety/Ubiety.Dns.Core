/*
 * http://www.iana.org/assignments/dns-parameters
 * 
 * 
 * 
 */


namespace Heijden.DNS
{
    /*
     * 3.2.2. TYPE values
     *
     * TYPE fields are used in resource records.
     * Note that these types are a subset of QTYPEs.
     *
     *        TYPE        value            meaning
     */
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

    /*
     * 3.2.3. QTYPE values
     *
     * QTYPE fields appear in the question part of a query.  QTYPES are a
     * superset of TYPEs, hence all TYPEs are valid QTYPEs.  In addition, the
     * following QTYPEs are defined:
     *
     *        QTYPE        value            meaning
     */
        /// <summary>
        /// </summary>
    public enum QType : ushort
    {
        /// <summary>
        /// </summary>
        A = RecordType.A,            // a IPV4 host address
        /// <summary>
        /// </summary>
        NS = RecordType.NS,        // an authoritative name server
        /// <summary>
        /// </summary>
        MD = RecordType.MD,        // a mail destination (Obsolete - use MX)
        /// <summary>
        /// </summary>
        MF = RecordType.MF,        // a mail forwarder (Obsolete - use MX)
        /// <summary>
        /// </summary>
        CNAME = RecordType.CNAME,    // the canonical name for an alias
        /// <summary>
        /// </summary>
        SOA = RecordType.SOA,        // marks the start of a zone of authority
        /// <summary>
        /// </summary>
        MB = RecordType.MB,        // a mailbox domain name (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        MG = RecordType.MG,        // a mail group member (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        MR = RecordType.MR,        // a mail rename domain name (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        NULL = RecordType.NULL,    // a null RR (EXPERIMENTAL)
        /// <summary>
        /// </summary>
        WKS = RecordType.WKS,        // a well known service description
        /// <summary>
        /// </summary>
        PTR = RecordType.PTR,        // a domain name pointer
        /// <summary>
        /// </summary>
        HINFO = RecordType.HINFO,    // host information
        /// <summary>
        /// </summary>
        MINFO = RecordType.MINFO,    // mailbox or mail list information
        /// <summary>
        /// </summary>
        MX = RecordType.MX,        // mail exchange
        /// <summary>
        /// </summary>
        TXT = RecordType.TXT,        // text strings

        /// <summary>
        /// </summary>
        RP = RecordType.RP,        // The Responsible Person rfc1183
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
        NIMLOC = RecordType.NIMLOC,// *** Nimrod Locator (Patton)

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
    /*
     * 3.2.4. CLASS values
     *
     * CLASS fields appear in resource records.  The following CLASS mnemonics
     *and values are defined:
     *
     *        CLASS        value            meaning
     */
        /// <summary>
        /// </summary>
    public enum Class : ushort
    {
        /// <summary>
        /// </summary>
        IN = 1,                // the Internet
        /// <summary>
        /// </summary>
        CS = 2,                // the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        /// <summary>
        /// </summary>
        CH = 3,                // the CHAOS class
        /// <summary>
        /// </summary>
        HS = 4                // Hesiod [Dyer 87]
    }
    /*
     * 3.2.5. QCLASS values
     *
     * QCLASS fields appear in the question section of a query.  QCLASS values
     * are a superset of CLASS values; every CLASS is a valid QCLASS.  In
     * addition to CLASS values, the following QCLASSes are defined:
     *
     *        QCLASS        value            meaning
     */
        /// <summary>
        /// </summary>
    public enum QClass : ushort
    {
        /// <summary>
        /// </summary>
        IN = Class.IN,        // the Internet
        /// <summary>
        /// </summary>
        CS = Class.CS,        // the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        /// <summary>
        /// </summary>
        CH = Class.CH,        // the CHAOS class
        /// <summary>
        /// </summary>
        HS = Class.HS,        // Hesiod [Dyer 87]

        /// <summary>
        /// </summary>
        ANY = 255            // any class
    }

    /*
RCODE           Response code - this 4 bit field is set as part of
                responses.  The values have the following
                interpretation:
     */
        /// <summary>
        /// </summary>
    public enum RCode
    {
        /// <summary>
        /// </summary>
        NoError = 0,        // No Error                           [RFC1035]
        /// <summary>
        /// </summary>
        FormErr = 1,        // Format Error                       [RFC1035]
        /// <summary>
        /// </summary>
        ServFail = 2,        // Server Failure                     [RFC1035]
        /// <summary>
        /// </summary>
        NXDomain = 3,        // Non-Existent Domain                [RFC1035]
        /// <summary>
        /// </summary>
        NotImp = 4,            // Not Implemented                    [RFC1035]
        /// <summary>
        /// </summary>
        Refused = 5,        // Query Refused                      [RFC1035]
        /// <summary>
        /// </summary>
        YXDomain = 6,        // Name Exists when it should not     [RFC2136]
        /// <summary>
        /// </summary>
        YXRRSet = 7,        // RR Set Exists when it should not   [RFC2136]
        /// <summary>
        /// </summary>
        NXRRSet = 8,        // RR Set that should exist does not  [RFC2136]
        /// <summary>
        /// </summary>
        NotAuth = 9,        // Server Not Authoritative for zone  [RFC2136]
        /// <summary>
        /// </summary>
        NotZone = 10,        // Name not contained in zone         [RFC2136]

        /// <summary>
        /// </summary>
        RESERVED11 = 11,    // Reserved
        /// <summary>
        /// </summary>
        RESERVED12 = 12,    // Reserved
        /// <summary>
        /// </summary>
        RESERVED13 = 13,    // Reserved
        /// <summary>
        /// </summary>
        RESERVED14 = 14,    // Reserved
        /// <summary>
        /// </summary>
        RESERVED15 = 15,    // Reserved

        /// <summary>
        /// </summary>
        BADVERSSIG = 16,    // Bad OPT Version                    [RFC2671]
                            // TSIG Signature Failure             [RFC2845]
        /// <summary>
        /// </summary>
        BADKEY = 17,        // Key not recognized                 [RFC2845]
        /// <summary>
        /// </summary>
        BADTIME = 18,        // Signature out of time window       [RFC2845]
        /// <summary>
        /// </summary>
        BADMODE = 19,        // Bad TKEY Mode                      [RFC2930]
        /// <summary>
        /// </summary>
        BADNAME = 20,        // Duplicate key name                 [RFC2930]
        /// <summary>
        /// </summary>
        BADALG = 21,        // Algorithm not supported            [RFC2930]
        /// <summary>
        /// </summary>
        BADTRUNC = 22        // Bad Truncation                     [RFC4635]
        /*
            23-3840              available for assignment
                0x0016-0x0F00
            3841-4095            Private Use
                0x0F01-0x0FFF
            4096-65535           available for assignment
                0x1000-0xFFFF
        */

    }

    /*
OPCODE          A four bit field that specifies kind of query in this
                message.  This value is set by the originator of a query
                and copied into the response.  The values are:

                0               a standard query (QUERY)

                1               an inverse query (IQUERY)

                2               a server status request (STATUS)

                3-15            reserved for future use
     */
        /// <summary>
        /// </summary>
    public enum OPCode
    {
        /// <summary>
        /// </summary>
        Query = 0,                // a standard query (QUERY)
        /// <summary>
        /// </summary>
        IQUERY = 1,                // OpCode Retired (previously IQUERY - No further [RFC3425]
                                // assignment of this code available)
        /// <summary>
        /// </summary>
        Status = 2,                // a server status request (STATUS) RFC1035
        /// <summary>
        /// </summary>
        RESERVED3 = 3,            // IANA

        /// <summary>
        /// </summary>
        Notify = 4,                // RFC1996
        /// <summary>
        /// </summary>
        Update = 5,                // RFC2136

        /// <summary>
        /// </summary>
        RESERVED6 = 6,
        /// <summary>
        /// </summary>
        RESERVED7 = 7,
        /// <summary>
        /// </summary>
        RESERVED8 = 8,
        /// <summary>
        /// </summary>
        RESERVED9 = 9,
        /// <summary>
        /// </summary>
        RESERVED10 = 10,
        /// <summary>
        /// </summary>
        RESERVED11 = 11,
        /// <summary>
        /// </summary>
        RESERVED12 = 12,
        /// <summary>
        /// </summary>
        RESERVED13 = 13,
        /// <summary>
        /// </summary>
        RESERVED14 = 14,
        /// <summary>
        /// </summary>
        RESERVED15 = 15,
    }
}
