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

namespace Ubiety.Dns.Core.Common.Extensions;

/// <summary>
/// Provides extension methods for working with <see cref="ushort" /> values, such as retrieving bytes,
/// setting flag values, and extracting flag information.
/// </summary>
public static class ShortExtensions
{
    /// <summary>
    /// Retrieves the byte representation of a <see cref="ushort" /> value.
    /// </summary>
    /// <param name="value"><see cref="ushort" /> value to convert to a byte array.</param>
    /// <returns><see cref="IEnumerable{T}" /> where T is <see cref="byte" />, representing the bytes of the given value.</returns>
    public static IEnumerable<byte> GetBytes(this ushort value)
    {
        return BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value));
    }

    /// <summary>
    /// Sets a flag value at the specified position within a <see cref="ushort" /> value.
    /// </summary>
    /// <param name="value"><see cref="ushort" /> to set the flag value on.</param>
    /// <param name="position">The position of the flag to set.</param>
    /// <param name="flagValue">The value of the flag, where true represents 1 and false represents 0.</param>
    /// <returns>A <see cref="ushort" /> with the specified flag value set at the given position.</returns>
    public static ushort SetFlag(this ushort value, int position, bool flagValue)
    {
        return value.SetFlag(position, 1, flagValue ? (ushort)1 : (ushort)0);
    }

    /// <summary>
    /// Sets a specific flag or flags within a <see cref="ushort" /> value by updating the bit(s) at a designated position.
    /// </summary>
    /// <param name="value"><see cref="ushort" /> to set the flag value on.</param>
    /// <param name="position">The starting bit position where the flag will be set.</param>
    /// <param name="length">The number of bits representing the flag value.</param>
    /// <param name="flagValue">The new value to set for the specified flag.</param>
    /// <returns><see cref="ushort" /> with the specified flag value set.</returns>
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
    /// Retrieves the value of a flag at the specified position within a <see cref="ushort" />.
    /// </summary>
    /// <param name="value"><see cref="ushort" /> from which to extract the flag.</param>
    /// <param name="position">The bit position of the flag to retrieve.</param>
    /// <returns><see cref="bool" /> indicating the state of the flag at the specified position.</returns>
    public static bool GetFlag(this ushort value, int position)
    {
        return value.GetFlag(position, 1) == 1;
    }

    /// <summary>
    /// Retrieves the value of a specified flag from a <see cref="ushort" />.
    /// </summary>
    /// <param name="value"><see cref="ushort" /> from which the flag's value will be retrieved.</param>
    /// <param name="position">Bit position within the <see cref="ushort" /> where the flag resides.</param>
    /// <param name="length">The number of bits representing the flag value.</param>
    /// <returns><see cref="bool" /> indicating the value of the flag at the specified position.</returns>
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