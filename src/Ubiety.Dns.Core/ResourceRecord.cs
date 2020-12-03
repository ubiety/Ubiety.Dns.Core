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
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Common.Extensions;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Dns.Core
{
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
    ///     Resource Record (rfc1034 3.6.)
    /// </summary>
    public class ResourceRecord
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceRecord" /> class.
        /// </summary>
        /// <param name="reader">Record reader of the record data.</param>
        protected ResourceRecord(RecordReader reader)
        {
            reader = reader.ThrowIfNull(nameof(reader));
            Name = reader.ReadDomainName();
            Type = (RecordType)reader.ReadUInt16();
            Class = (OperationClass)reader.ReadUInt16();
            TimeToLive = reader.ReadUInt32();
            RecordLength = reader.ReadUInt16();
            Record = reader.ReadRecord(Type);
            Record.ResourceRecord = this;
        }

        /// <summary>
        ///     Gets the name of the node to which this resource record pertains.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the type of resource record.
        /// </summary>
        public RecordType Type { get; }

        /// <summary>
        ///     Gets the type class of resource record, mostly IN but can be CS, CH or HS.
        /// </summary>
        public OperationClass Class { get; }

        /// <summary>
        ///     Gets the time to live, in seconds, that the resource record may be cached.
        /// </summary>
        public uint TimeToLive { get; }

        /// <summary>
        ///     Gets the record length.
        /// </summary>
        public ushort RecordLength { get; }

        /// <summary>
        ///     Gets one of the Record* classes.
        /// </summary>
        public Record Record { get; }

        /// <summary>
        ///     Is the record expired according to the response timestamp.
        /// </summary>
        /// <param name="responseTimeStamp">Timestamp from the response for the record.</param>
        /// <returns>True if the record is expired; otherwise false.</returns>
        public bool IsExpired(DateTime responseTimeStamp)
        {
            var timeLived = (int)((DateTime.Now.Ticks - responseTimeStamp.Ticks) / TimeSpan.TicksPerSecond);

            return (uint)Math.Max(0, TimeToLive - timeLived) == 0;
        }

        /// <summary>
        ///     String version of the resource record.
        /// </summary>
        /// <returns>String of the resource.</returns>
        public override string ToString()
        {
            return $"{Name,-32} {TimeToLive}\t{Class}\t{Type}\t{Record}";
        }
    }
}
