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

using System;
using System.Collections.Generic;
using System.Net;

namespace Ubiety.Dns.Core.Common.Extensions
{
    /// <summary>
    ///     Extension methods for shorts.
    /// </summary>
    public static class ShortExtensions
    {
        /// <summary>
        ///     Get a byte array of the short.
        /// </summary>
        /// <param name="value"><see cref="ushort" /> to get the bytes for.</param>
        /// <returns><see cref="byte" /> array of the short.</returns>
        public static IEnumerable<byte> GetBytes(this ushort value)
        {
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value));
        }
    }
}