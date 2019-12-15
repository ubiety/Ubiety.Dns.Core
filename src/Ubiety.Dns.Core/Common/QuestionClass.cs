/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System.Diagnostics.CodeAnalysis;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Question class.
    /// </summary>
    public enum QuestionClass
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
        Any = 255,
    }
}