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
        ///     Mail destination type (Obsolete - Use MX)
        /// </summary>
        MD = RecordType.MD,

        /// <summary>
        ///     Mail forwarder type (Obsolete - use MX)
        /// </summary>
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
        ///     Network service access point pointer (Obsolete)
        /// </summary>
        NSAPPTR = RecordType.NSAPPTR,

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
        ///     Global position DNS type (Obsolete)
        /// </summary>
        GPOS = RecordType.GPOS,

        /// <summary>
        ///     IPv6 address DNS type
        /// </summary>
        AAAA = RecordType.AAAA,

        /// <summary>
        ///     DNS location information
        /// </summary>
        LOC = RecordType.LOC,

        /// <summary>
        ///     Obsolete DNS type
        /// </summary>
        NXT = RecordType.NXT,

        /// <summary>
        ///     Endpoint identifier
        /// </summary>
        EID = RecordType.EID,

        /// <summary>
        ///     Nimrod locator
        /// </summary>
        NIMLOC = RecordType.NIMLOC,

        /// <summary>
        ///     Location of services
        /// </summary>
        SRV = RecordType.SRV,

        /// <summary>
        ///     ATM address
        /// </summary>
        ATMA = RecordType.ATMA,

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
        ///     IPv6 address type (Historic)
        /// </summary>
        A6 = RecordType.A6,

        /// <summary>
        ///     DNS name redirection
        /// </summary>
        DNAME = RecordType.DNAME,

        /// <summary>
        ///     SINK DNS type
        /// </summary>
        SINK = RecordType.SINK,

        /// <summary>
        ///     OPT DNS type
        /// </summary>
        OPT = RecordType.OPT,

        /// <summary>
        ///     APL DNS type
        /// </summary>
        APL = RecordType.APL,

        /// <summary>
        ///     Delegation signer DNS type
        /// </summary>
        DS = RecordType.DS,

        /// <summary>
        ///     SSH key fingerprint
        /// </summary>
        SSHFP = RecordType.SSHFP,

        /// <summary>
        ///     IPSEC key DNS type
        /// </summary>
        IPSECKEY = RecordType.IPSECKEY,

        /// <summary>
        ///     Resource record signature
        /// </summary>
        RRSIG = RecordType.RRSIG,

        /// <summary>
        ///     NSEC DNS type
        /// </summary>
        NSEC = RecordType.NSEC,

        /// <summary>
        ///     DNSKEY DNS type
        /// </summary>
        DNSKEY = RecordType.DNSKEY,

        /// <summary>
        ///     DHCP identifier type
        /// </summary>
        DHCID = RecordType.DHCID,

        /// <summary>
        ///     NSEC3 DNS type
        /// </summary>
        NSEC3 = RecordType.NSEC3,

        /// <summary>
        ///     NSEC3PARAM DNS type
        /// </summary>
        NSEC3PARAM = RecordType.NSEC3PARAM,

        /// <summary>
        ///     HIP DNS type
        /// </summary>
        HIP = RecordType.HIP,

        /// <summary>
        ///     SPF DNS type
        /// </summary>
        SPF = RecordType.SPF,

        /// <summary>
        ///     UINFO DNS type
        /// </summary>
        UINFO = RecordType.UINFO,

        /// <summary>
        ///     UID DNS type
        /// </summary>
        UID = RecordType.UID,

        /// <summary>
        ///     GID DNS type
        /// </summary>
        GID = RecordType.GID,

        /// <summary>
        ///     UNSPEC DNS type
        /// </summary>
        UNSPEC = RecordType.UNSPEC,

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

        /// <summary>
        ///     DNSSEC trust authorities
        /// </summary>
        TA = RecordType.TA,

        /// <summary>
        ///     DNSSEC lookaside validation
        /// </summary>
        DLV = RecordType.DLV,
    }
}