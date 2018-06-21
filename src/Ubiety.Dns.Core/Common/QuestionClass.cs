using System.Diagnostics.CodeAnalysis;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Question class
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1717", Justification = "Class is not plural")]
    public enum QuestionClass : ushort
    {
        /// <summary>
        ///     Internet class
        /// </summary>
        IN = OperationClass.IN,

        /// <summary>
        ///     CSNET class
        /// </summary>
        CS = OperationClass.CS,

        /// <summary>
        ///     CHAOS class
        /// </summary>
        CH = OperationClass.CH,

        /// <summary>
        ///     Hesiod class
        /// </summary>
        HS = OperationClass.HS,

        /// <summary>
        ///     Any question class
        /// </summary>
        Any = 255
    }
}