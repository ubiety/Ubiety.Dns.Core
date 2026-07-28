/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
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
/// Specifies the operation codes used in DNS messages.
/// </summary>
public enum OperationCode
{
    /// <summary>
    /// Represents a standard DNS query operation code.
    /// </summary>
    Query = 0,

    /// <summary>
    /// Represents an inverse query DNS operation code.
    /// </summary>
    IQuery = 1,

    /// <summary>
    /// Represents a DNS operation code indicating a status request.
    /// </summary>
    Status = 2,

    /// <summary>
    /// Represents a DNS "Notify" operation code, used to inform secondary servers of zone changes.
    /// </summary>
    Notify = 4,

    /// <summary>
    /// Represents an operation code for updating DNS records.
    /// </summary>
    Update = 5,
}
