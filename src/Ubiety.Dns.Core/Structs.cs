/*
 * http://www.iana.org/assignments/dns-parameters
 * 
 * 
 * 
 */


namespace Ubiety.Dns.Core
{
    /*
     * 3.2.2. TYPE values
     *
     * TYPE fields are used in resource records.
     * Note that these types are a subset of QTYPEs.
     *
     *        TYPE        value            meaning
     */

    /*
     * 3.2.3. QTYPE values
     *
     * QTYPE fields appear in the question part of a query.  QTYPES are a
     * superset of TYPEs, hence all TYPEs are valid QTYPEs.  In addition, the
     * following QTYPEs are defined:
     *
     *        QTYPE        value            meaning
     */
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
