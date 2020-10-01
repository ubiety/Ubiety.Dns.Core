/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
            return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value));
        }

        /// <summary>
        ///     Set a ushort flag value.
        /// </summary>
        /// <param name="value"><see cref="ushort" /> to set the flag value on.</param>
        /// <param name="position">Position of the flag.</param>
        /// <param name="flagValue">Value of the flag.</param>
        /// <returns><see cref="ushort" /> with the flag value set.</returns>
        public static ushort SetFlag(this ushort value, int position, bool flagValue)
        {
            return value.SetFlag(position, 1, flagValue ? (ushort)1 : (ushort)0);
        }

        /// <summary>
        ///     Set a ushort flag value.
        /// </summary>
        /// <param name="value"><see cref="ushort" /> to set the flag value on.</param>
        /// <param name="position">Position of the flag.</param>
        /// <param name="length">Length of the flag.</param>
        /// <param name="flagValue">Value of the flag.</param>
        /// <returns><see cref="ushort" /> with the flag value set.</returns>
        public static ushort SetFlag(this ushort value, int position, int length, ushort flagValue)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return value;
            }

            // get some mask to put on
            var mask = (2 << (length - 1)) - 1;

            // clear out value
            value &= (ushort)~(mask << position);

            // set new value
            value |= (ushort)((flagValue & mask) << position);
            return value;
        }

        /// <summary>
        ///     Get the value of a boolean flag.
        /// </summary>
        /// <param name="value"><see cref="ushort" /> to get the value from.</param>
        /// <param name="position">Position of the value.</param>
        /// <returns><see cref="bool" /> of the flag.</returns>
        public static bool GetFlag(this ushort value, int position)
        {
            return value.GetFlag(position, 1) == 1;
        }

        /// <summary>
        ///     Get the value of a flag.
        /// </summary>
        /// <param name="value"><see cref="ushort" /> to get the value from.</param>
        /// <param name="position">Position of the value.</param>
        /// <param name="length">Length of the value.</param>
        /// <returns><see cref="ushort" /> of the value.</returns>
        public static ushort GetFlag(this ushort value, int position, int length)
        {
            // sanity check
            if (length <= 0 || position >= 16)
            {
                return 0;
            }

            // get some mask to put on
            var mask = (2 << (length - 1)) - 1;

            // shift down to get some value and mask it
            return (ushort)((value >> position) & mask);
        }
    }
}
