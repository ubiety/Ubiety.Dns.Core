namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     DNS Server response code
    /// </summary>
    public enum ResponseCode
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
    }
}