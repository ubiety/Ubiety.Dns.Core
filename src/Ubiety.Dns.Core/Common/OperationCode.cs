/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     DNS Record OpCode.
    /// </summary>
    public enum OperationCode
    {
        /// <summary>
        ///     Standard DNS Query
        /// </summary>
        Query = 0,

        /// <summary>
        ///     Retired IQUERY code
        /// </summary>
        IQuery = 1,

        /// <summary>
        ///     Server status request
        /// </summary>
        Status = 2,

        /// <summary>
        ///     Notify OpCode
        /// </summary>
        Notify = 4,

        /// <summary>
        ///     Update OpCode
        /// </summary>
        Update = 5,
    }
}