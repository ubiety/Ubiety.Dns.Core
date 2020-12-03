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

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     DNS Record OpCode.
    /// </summary>
    public enum OperationCode
    {
        /// <summary>
        ///     Standard DNS Query
        /// </summary>
        Query = 0,

        /// <summary>
        ///     Retired IQUERY code
        /// </summary>
        IQuery = 1,

        /// <summary>
        ///     Server status request
        /// </summary>
        Status = 2,

        /// <summary>
        ///     Notify OpCode
        /// </summary>
        Notify = 4,

        /// <summary>
        ///     Update OpCode
        /// </summary>
        Update = 5,
    }
}
