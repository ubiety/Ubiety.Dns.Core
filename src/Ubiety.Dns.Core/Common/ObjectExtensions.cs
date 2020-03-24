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

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///    Object class extension methods.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Checks for null and throws an exception.
        /// </summary>
        /// <param name="target">Object to check for null.</param>
        /// <param name="name">Name of the parameter.</param>
        /// <typeparam name="T">Type of object to check.</typeparam>
        /// <returns>Target object if not null.</returns>
        public static T ThrowIfNull<T>(this T target, string name)
            where T : class
        {
            if (target is null)
            {
                throw new ArgumentNullException(name);
            }

            return target;
        }
    }
}