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
using System.Reflection;

using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Core.Common.Extensions;

/// <summary>
/// Provides extension methods for operations related to enumerations in the DNS core library.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Retrieves a record instance of the specified type, using the provided resource reader and optional length.
    /// </summary>
    /// <param name="type">The type of the DNS record to retrieve.</param>
    /// <param name="reader">The resource reader used to create the record.</param>
    /// <param name="length">The length of the record, used for certain record types. Defaults to 0.</param>
    /// <returns>An instance of the <see cref="Record"/> class for the specified record type.</returns>
    public static Record GetRecord(this RecordType type, RecordReader reader, int length = 0)
    {
        var fieldInfo = type.GetType().GetField(type.ToString());
        var recordAttr = fieldInfo?.GetCustomAttribute<RecordAttribute>();

        // CreateInstance only returns null when asked for a Nullable<T>; every RecordType
        // registered by RecordAttribute is a class, so the result is always a Record.
        if (type == RecordType.TXT)
        {
            return (Record)Activator.CreateInstance(recordAttr?.RecordType ?? throw new InvalidOperationException(), reader, length)!;
        }

        return (Record)Activator.CreateInstance(recordAttr?.RecordType ?? throw new InvalidOperationException(), reader)!;
    }
}
