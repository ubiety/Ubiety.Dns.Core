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
    ///     DNS Server response code.
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        ///     No error in query
        /// </summary>
        NoError = 0,

        /// <summary>
        ///     Format error in query
        /// </summary>
        FormErr = 1,

        /// <summary>
        ///     Server failure
        /// </summary>
        ServFail = 2,

        /// <summary>
        ///     Non-Existent Domain
        /// </summary>
        NXDomain = 3,

        /// <summary>
        ///     Not implemented
        /// </summary>
        NotImp = 4,

        /// <summary>
        ///     Query refused
        /// </summary>
        Refused = 5,

        /// <summary>
        ///     Name exists when it should not
        /// </summary>
        YXDomain = 6,

        /// <summary>
        ///     RR set exists when it should not
        /// </summary>
        YXRRSet = 7,

        /// <summary>
        ///     RR set that exists does not
        /// </summary>
        NXRRSet = 8,

        /// <summary>
        ///     Server not authoritative for zone
        /// </summary>
        NotAuth = 9,

        /// <summary>
        ///     Name not contained in zone
        /// </summary>
        NotZone = 10,

        /// <summary>
        ///     Bad OPT version
        /// </summary>
        BADVERSSIG = 16,

        /// <summary>
        ///     Key not recognized
        /// </summary>
        BADKEY = 17,

        /// <summary>
        ///     Signature out of time window
        /// </summary>
        BADTIME = 18,

        /// <summary>
        ///     Bad TKEY mode
        /// </summary>
        BADMODE = 19,

        /// <summary>
        ///     Duplicate key name
        /// </summary>
        BADNAME = 20,

        /// <summary>
        ///     Algorithm not supported
        /// </summary>
        BADALG = 21,

        /// <summary>
        ///     Bad truncation
        /// </summary>
        BADTRUNC = 22,
    }
}
