using System;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Question class
    /// </summary>
    public enum QuestionClass : UInt16
    {
        /// <summary>
        ///     Internet class
        /// </summary>
        IN = Class.IN,

        /// <summary>
        ///     CSNET class
        /// </summary>
        CS = Class.CS,

        /// <summary>
        ///     CHAOS class
        /// </summary>
        CH = Class.CH,

        /// <summary>
        ///     Hesiod class
        /// </summary>
        HS = Class.HS,

        /// <summary>
        ///     Any question class
        /// </summary>
        Any = 255
    }
}