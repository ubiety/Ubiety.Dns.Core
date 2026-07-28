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
/// Represents the response codes used in DNS queries and responses.
/// </summary>
public enum ResponseCode
{
    /// <summary>
    /// Indicates that no error occurred during the DNS query or response.
    /// </summary>
    NoError = 0,

    /// <summary>
    /// Indicates a format error, meaning the server was unable to interpret the query due to a format issue.
    /// </summary>
    FormErr = 1,

    /// <summary>
    /// Indicates a server failure occurred, meaning the DNS server was unable to process the query due to an internal error.
    /// </summary>
    ServFail = 2,

    /// <summary>
    /// Indicates that the domain name referenced in the DNS query does not exist.
    /// </summary>
    NXDomain = 3,

    /// <summary>
    /// Indicates that the requested operation is not implemented on the DNS server.
    /// </summary>
    NotImp = 4,

    /// <summary>
    /// Indicates that the DNS server refused to process the query for policy reasons.
    /// </summary>
    Refused = 5,

    /// <summary>
    /// Indicates that a name exists when it was not expected during a DNS query, typically in relation to
    /// zone integrity checks.
    /// </summary>
    YXDomain = 6,

    /// <summary>
    /// Indicates that an RR (Resource Record) set exists when it should not during the DNS query or response.
    /// </summary>
    YXRRSet = 7,

    /// <summary>
    /// Indicates that the name exists, but no associated resource records (RR) of the requested type were found.
    /// Typically returned in DNS queries when the requested RR type is not present for the queried name.
    /// </summary>
    NXRRSet = 8,

    /// <summary>
    /// Indicates that the server is not authoritative for the requested domain.
    /// </summary>
    NotAuth = 9,

    /// <summary>
    /// Indicates that the specified zone is invalid for a DNS query or update.
    /// </summary>
    NotZone = 10,

    /// <summary>
    /// Indicates that the signature provided in a DNSSEC-enabled response is invalid due to a version or format issue.
    /// </summary>
    BADVERSSIG = 16,

    /// <summary>
    /// Indicates that the key used in the DNS query or response is invalid.
    /// </summary>
    BADKEY = 17,

    /// <summary>
    /// Indicates a bad or out-of-date timestamp was provided during a DNS query or response, typically related to the TSIG authentication.
    /// </summary>
    BADTIME = 18,

    /// <summary>
    /// Indicates that the server is in a bad operating mode, which does not support the DNS operation.
    /// </summary>
    BADMODE = 19,

    /// <summary>
    /// Indicates that an invalid or malformed name was encountered in the DNS query or response.
    /// </summary>
    BADNAME = 20,

    /// <summary>
    /// Indicates that a bad algorithm was specified in the DNS query or response.
    /// </summary>
    BADALG = 21,

    /// <summary>
    /// Indicates that a BADTRUNC error occurred, which is related to a malformed or improperly truncated DNS message.
    /// </summary>
    BADTRUNC = 22,
}
