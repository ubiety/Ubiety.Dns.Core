namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    /// </summary>
    public enum OperationCode
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
