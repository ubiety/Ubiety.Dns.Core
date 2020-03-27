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
using System.Linq;

namespace Ubiety.Dns.Core.Common.Helpers
{
    /// <summary>
    ///     Provides methods to assist with implementing IEquatable.
    /// </summary>
    /// <typeparam name="T">Type of class to use.</typeparam>
    internal class EqualityHelper<T>
    {
        private readonly Func<T, object>[] _fields;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EqualityHelper{T}" /> class.
        /// </summary>
        /// <param name="fields">Fields to use for calculating equality.</param>
        public EqualityHelper(params Func<T, object>[] fields)
        {
            _fields = fields;
        }

        /// <summary>
        ///     Determines whether the specified instances are equal.
        /// </summary>
        /// <param name="left">First instance to compare.</param>
        /// <param name="right">Second instance to compare.</param>
        /// <returns>True if the objects are equal, otherwise false.</returns>
        public bool Equals(T left, T right)
        {
            if (left is null || right is null)
            {
                return false;
            }

            if (ReferenceEquals(left, right))
            {
                return true;
            }

            return _fields.All(field => Equals(field(left), field(right)));
        }

        /// <summary>
        ///     Calculates the hash code of the object.
        /// </summary>
        /// <param name="instance">Object instance to calculate the hash code of.</param>
        /// <returns>A hash code.</returns>
        public int GetHashCode(T instance)
        {
            var hashCode = GetType().GetHashCode();

            unchecked
            {
                hashCode = _fields.Select(field => field(instance)).Aggregate(hashCode, (current, item) => (current * 450) ^ (item is null ? 0 : item.GetHashCode()));
            }

            return hashCode;
        }
    }
}