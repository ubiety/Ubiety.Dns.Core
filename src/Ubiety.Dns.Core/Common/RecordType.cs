namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     DNS record type
    /// </summary>
    public enum RecordType
    {
        /// <summary>
        ///     IPv4 address
        /// </summary>
        A = 1,

        /// <summary>
        ///     Authoritative nameserver
        /// </summary>
        NS = 2,

        /// <summary>
        ///     Mail destination
        /// </summary>
        MD = 3,

        /// <summary>
        ///     Mail forwarder
        /// </summary>
        MF = 4,

        /// <summary>
        ///     Canonical name for a domain alias
        /// </summary>
        CNAME = 5,

        /// <summary>
        ///     The start of a zone of authority
        /// </summary>
        SOA = 6,

        /// <summary>
        ///     Mailbox domain name
        /// </summary>
        MB = 7,

        /// <summary>
        ///     Mail group member
        /// </summary>
        MG = 8,

        /// <summary>
        ///     Mail rename domain
        /// </summary>
        MR = 9,

        /// <summary>
        ///     Null resource record
        /// </summary>
        NULL = 10,

        /// <summary>
        ///     Well known service description
        /// </summary>
        WKS = 11,

        /// <summary>
        ///     Domain pointer type
        /// </summary>
        PNTR = 12,

        /// <summary>
        ///     Host information type
        /// </summary>
        HINFO = 13,

        /// <summary>
        ///     Mailbox or list information
        /// </summary>
        MINFO = 14,

        /// <summary>
        ///     Mail exchange type
        /// </summary>
        MX = 15,

        /// <summary>
        ///     Text string
        /// </summary>
        TXT = 16,

        /// <summary>
        ///     Responsible person type
        /// </summary>
        RP = 17,

        /// <summary>
        ///     AFS database location
        /// </summary>
        AFSDB = 18,

        /// <summary>
        ///     X.25 address type
        /// </summary>
        X25 = 19,

        /// <summary>
        ///     ISDN address type
        /// </summary>
        ISDN = 20,

        /// <summary>
        ///     Route through DNS type
        /// </summary>
        RT = 21,

        /// <summary>
        ///     Network service access point address
        /// </summary>
        NSAP = 22,

        /// <summary>
        ///     Obsolete DNS type
        /// </summary>
        NSAPPTR = 23,

        /// <summary>
        ///     Cryptographic public key signature
        /// </summary>
        SIG = 24,

        /// <summary>
        ///     DNSSEC public key
        /// </summary>
        KEY = 25,

        /// <summary>
        ///     Pointer for X.400 mail mapping information
        /// </summary>
        PX = 26,

        /// <summary>
        ///     Geographical position type
        /// </summary>
        GPOS = 27,

        /// <summary>
        ///     IPv6 host address
        /// </summary>
        AAAA = 28,

        /// <summary>
        ///     Location information
        /// </summary>
        LOC = 29,

        /// <summary>
        ///     Next domain type
        /// </summary>
        NXT = 30,

        /// <summary>
        ///     Endpoint identifier
        /// </summary>
        EID = 31,

        /// <summary>
        ///     Nimrod locator
        /// </summary>
        NIMLOC = 32,

        /// <summary>
        ///     Location of services
        /// </summary>
        SRV = 33,

        /// <summary>
        ///     ATM address type
        /// </summary>
        ATMA = 34,

        /// <summary>
        ///     Naming authority pointer
        /// </summary>
        NAPTR = 35,

        /// <summary>
        ///     Key exchange record type
        /// </summary>
        KX = 36,

        /// <summary>
        ///     Certificate
        /// </summary>
        CERT = 37,

        /// <summary>
        ///     IPv6 address (Historic)
        /// </summary>
        A6 = 38,

        /// <summary>
        ///     Domain alias
        /// </summary>
        DNAME = 39,

        /// <summary>
        ///     SINK type
        /// </summary>
        SINK = 40,

        /// <summary>
        ///     OPT type
        /// </summary>
        OPT = 41,

        /// <summary>
        ///     APL type
        /// </summary>
        APL = 42,

        /// <summary>
        ///     Delegation signer
        /// </summary>
        DS = 43,

        /// <summary>
        ///     SSH key fingerprint
        /// </summary>
        SSHFP = 44,

        /// <summary>
        ///     IPSEC key
        /// </summary>
        IPSECKEY = 45,

        /// <summary>
        ///     Resource record signature
        /// </summary>
        RRSIG = 46,

        /// <summary>
        ///     NSEC type
        /// </summary>
        NSEC = 47,

        /// <summary>
        ///     DNS key
        /// </summary>
        DNSKEY = 48,

        /// <summary>
        ///     DHCP information
        /// </summary>
        DHCID = 49,

        /// <summary>
        ///     NSEC3 type
        /// </summary>
        NSEC3 = 50,

        /// <summary>
        ///     NSEC3PARAM type
        /// </summary>
        NSEC3PARAM = 51,

        /// <summary>
        ///     Host Identity protocol
        /// </summary>
        HIP = 55,

        /// <summary>
        ///     SPF type
        /// </summary>
        SPF = 99,

        /// <summary>
        ///     Reserved
        /// </summary>
        UINFO = 100,

        /// <summary>
        ///     Reserved
        /// </summary>
        UID = 101,

        /// <summary>
        ///     Reserved
        /// </summary>
        GID = 102,

        /// <summary>
        ///     Reserved
        /// </summary>
        UNSPEC = 103,

        /// <summary>
        ///     Transaction key
        /// </summary>
        TKEY = 249,

        /// <summary>
        ///     Transaction signature
        /// </summary>
        TSIG = 250,

        /// <summary>
        ///     DNSSEC trust authorities
        /// </summary>
        TA = 32768,

        /// <summary>
        ///     DNSSEC lookaside validation
        /// </summary>
        DLV = 32769
    }
}