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

namespace Ubiety.Dns.Core.Common.Extensions;

/// <summary>
/// Provides extension methods for object validation.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified object is null.
    /// </summary>
    /// <param name="target">The object to check for null.</param>
    /// <param name="name">The name of the parameter being validated.</param>
    /// <typeparam name="T">The type of the object to check.</typeparam>
    /// <returns>The non-null target object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the target object is null.</exception>
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