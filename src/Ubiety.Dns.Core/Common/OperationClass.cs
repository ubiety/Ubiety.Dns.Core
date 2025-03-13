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

namespace Ubiety.Dns.Core.Common;

/// <summary>
/// Specifies the operation class for DNS queries and resource records.
/// </summary>
public enum OperationClass
{
    /// <summary>
    /// Represents the Internet class used in DNS queries and resource records.
    /// </summary>
    IN = 1,

    /// <summary>
    /// Represents the CS (CSNET) class, which is an obsolete class formerly used in early networking contexts.
    /// </summary>
    CS = 2,

    /// <summary>
    /// Represents the Chaos class used in DNS queries and resource records.
    /// </summary>
    CH = 3,

    /// <summary>
    /// Represents the Hesiod class used in DNS queries and resource records.
    /// </summary>
    HS = 4,
}