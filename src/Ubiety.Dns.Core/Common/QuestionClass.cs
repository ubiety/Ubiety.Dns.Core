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
    /// Represents the class of a DNS question, specifying the scope or network context
    /// for a DNS query, such as Internet, CSNET, Chaos, or Hesiod.
    /// </summary>
    public enum QuestionClass
    {
        /// <summary>
        /// IN class, representing the Internet class for DNS queries.
        /// </summary>
        IN = OperationClass.IN,

        /// <summary>
        /// CS class, representing the CSNET (Computer Science Network) class for DNS queries.
        /// </summary>
        CS = OperationClass.CS,

        /// <summary>
        /// CH class, representing the Chaos network class for DNS queries.
        /// </summary>
        CH = OperationClass.CH,

        /// <summary>
        /// HS class, representing the Hesiod class for DNS queries.
        /// </summary>
        HS = OperationClass.HS,

        /// <summary>
        /// ANY class, representing a wildcard class that matches any DNS query class.
        /// </summary>
        Any = 255,
    }
}
