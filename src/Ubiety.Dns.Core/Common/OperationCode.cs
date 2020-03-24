/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
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