using System;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Resource record class
    /// </summary>
    public enum OperationClass : UInt16
    {
        /// <summary>
        ///     Internet class
        /// </summary>
        IN = 1,

        /// <summary>
        ///     CSNET class
        /// </summary>
        CS = 2,

        /// <summary>
        ///     CHAOS class
        /// </summary>
        CH = 3,

        /// <summary>
        ///     Hesiod class
        /// </summary>
        HS = 4
    }
}