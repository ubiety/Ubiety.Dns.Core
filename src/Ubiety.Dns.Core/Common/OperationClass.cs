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
    ///     Resource record class.
    /// </summary>
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