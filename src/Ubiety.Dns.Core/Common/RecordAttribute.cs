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

namespace Ubiety.Dns.Core.Common;

/// <summary>
/// Represents an attribute that is used to associate a DNS record type with a specific field.
/// </summary>
/// <remarks>
/// This attribute is applied to fields of an enumeration to define the mapping between a DNS record identifier
/// and the corresponding implementation type of the DNS record.
/// </remarks>
/// <example>
/// This class is used internally to dynamically instantiate record types for DNS parsing.
/// </example>
[AttributeUsage(AttributeTargets.Field)]
public class RecordAttribute(Type record) : Attribute
{
    /// <summary>
    ///     Gets the record type.
    /// </summary>
    public Type RecordType => record;
}