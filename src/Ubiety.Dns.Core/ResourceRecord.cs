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

using System;

using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core.Records;
/*
3.2. RR definitions

3.2.1. Format

All RRs have the same top level format shown below:

                                    1  1  1  1  1  1
      0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                                               |
    /                                               /
    /                      NAME                     /
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                      TYPE                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     CLASS                     |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                      TTL                      |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                   RDLENGTH                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
    /                     RDATA                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+


where:

NAME            an owner name, i.e., the name of the node to which this
                resource record pertains.

TYPE            two octets containing one of the RR TYPE codes.

CLASS           two octets containing one of the RR CLASS codes.

TTL             a 32 bit signed integer that specifies the time interval
                that the resource record may be cached before the source
                of the information should again be consulted.  Zero
                values are interpreted to mean that the RR can only be
                used for the transaction in progress, and should not be
                cached.  For example, SOA records are always distributed
                with a zero TTL to prohibit caching.  Zero values can
                also be used for extremely volatile data.

RDLENGTH        an unsigned 16 bit integer that specifies the length in
                octets of the RDATA field.

RDATA           a variable length string of octets that describes the
                resource.  The format of this information varies
                according to the TYPE and CLASS of the resource record.
*/

/// <summary>
/// Represents a DNS resource record as defined in RFC 1035.
/// </summary>
/// <remarks>
/// A resource record provides information about a DNS entity, including its name, type, class, time-to-live, record length, and resource data.
/// </remarks>
public class ResourceRecord
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResourceRecord" /> class.
    /// </summary>
    /// <param name="reader">Record reader of the record data.</param>
    protected ResourceRecord(RecordReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        Name = reader.ReadDomainName();
        Type = (RecordType)reader.ReadUInt16();
        Class = (OperationClass)reader.ReadUInt16();
        TimeToLive = reader.ReadUInt32();
        RecordLength = reader.ReadUInt16();
        Record = reader.ReadRecord(Type);
        Record.ResourceRecord = this;
    }

    /// <summary>
    /// Gets the owner name of the node to which this resource record pertains.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the resource record type, which defines the format of the data in the RDATA field.
    /// </summary>
    public RecordType Type { get; }

    /// <summary>
    /// Gets the class of the resource record, represented by a two-octet code as specified in the DNS RR CLASS definitions.
    /// </summary>
    public OperationClass Class { get; }

    /// <summary>
    /// Gets the time interval, in seconds, that the resource record may be cached before the source of the information must be consulted again.
    /// A zero value indicates the record cannot be cached and is valid only for the ongoing transaction.
    /// </summary>
    public uint TimeToLive { get; }

    /// <summary>
    /// Gets the length, in octets, of the resource data (RDATA) field for this resource record.
    /// </summary>
    public ushort RecordLength { get; }

    /// <summary>
    ///     Gets one of the Record* classes.
    /// </summary>
    public Record Record { get; }

    /// <summary>
    /// Determines whether the resource record is expired based on the response timestamp.
    /// </summary>
    /// <param name="responseTimeStamp">The UTC timestamp from the response for the record.</param>
    /// <returns>True if the resource record is expired; otherwise, false.</returns>
    /// <remarks>
    /// Compared in UTC to match <see cref="Response.TimeStamp"/>, so a daylight saving transition
    /// cannot expire a record an hour early or keep it an hour too long.
    /// </remarks>
    public bool IsExpired(DateTime responseTimeStamp)
    {
        var timeLived = (int)((DateTime.UtcNow.Ticks - responseTimeStamp.Ticks) / TimeSpan.TicksPerSecond);

        return (uint)Math.Max(0, TimeToLive - timeLived) == 0;
    }

    /// <summary>
    /// Returns a string representation of the resource record.
    /// </summary>
    /// <returns>A string containing the record's name, time-to-live, class, type, and associated record data.</returns>
    public override string ToString()
    {
        return $"{Name,-32} {TimeToLive}\t{Class}\t{Type}\t{Record}";
    }
}
