/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
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