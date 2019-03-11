/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System.Diagnostics.CodeAnalysis;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Resource record class.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1717", Justification = "Class is not plural")]
    public enum OperationClass
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
        HS = 4,
    }
}