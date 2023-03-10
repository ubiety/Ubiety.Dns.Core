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

namespace Ubiety.Dns.Core.Common.Extensions
{
    /// <summary>
    ///     Enumeration extension methods.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the record for the record type.
        /// </summary>
        /// <param name="type">Type of record to get.</param>
        /// <param name="reader">Resource reader to create record with.</param>
        /// <returns>A <see cref="Record"/> instance for the given type.</returns>
        public static Record GetRecord(this RecordType type, RecordReader reader)
        {
            var fieldInfo = type.GetType().GetField(type.ToString());
            var recordAttr = fieldInfo.GetCustomAttribute<RecordAttribute>();

            return (Record)Activator.CreateInstance(recordAttr.RecordType, reader);
        }
    }
}
