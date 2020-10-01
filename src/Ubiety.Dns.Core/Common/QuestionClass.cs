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
    ///     Question class.
    /// </summary>
    public enum QuestionClass
    {
        /// <summary>
        ///     Internet class
        /// </summary>
        IN = OperationClass.IN,

        /// <summary>
        ///     CSNET class
        /// </summary>
        CS = OperationClass.CS,

        /// <summary>
        ///     CHAOS class
        /// </summary>
        CH = OperationClass.CH,

        /// <summary>
        ///     Hesiod class
        /// </summary>
        HS = OperationClass.HS,

        /// <summary>
        ///     Any question class
        /// </summary>
        Any = 255,
    }
}
