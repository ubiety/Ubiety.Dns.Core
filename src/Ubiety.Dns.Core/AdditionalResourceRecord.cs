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

namespace Ubiety.Dns.Core
{
    /// <summary>
    /// Represents an additional DNS resource record in a DNS response.
    /// </summary>
    /// <remarks>
    /// This class provides functionality to parse and manage additional resource records
    /// in a DNS response. Additional resource records are often used to provide supplementary
    /// information related to the DNS query response but are not directly part of the
    /// answer, authority, or primary resource records.
    /// </remarks>
    public class AdditionalResourceRecord(RecordReader reader) : ResourceRecord(reader)
    {
    }
}
