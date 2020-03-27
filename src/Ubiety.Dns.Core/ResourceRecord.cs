/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
        private uint _ttl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceRecord" /> class.
        /// </summary>
        /// <param name="reader">Record reader of the record data.</param>
        protected ResourceRecord(RecordReader reader)
        {
            reader = reader.ThrowIfNull(nameof(reader));
            TimeLived = 0;
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
        ///     Gets the time to live, the time interval that the resource record may be cached.
        /// </summary>
        public uint TimeToLive
        {
            get => (uint)Math.Max(0, _ttl - TimeLived);

            private set => _ttl = value;
        }

        /// <summary>
        ///     Gets the record length.
        /// </summary>
        public ushort RecordLength { get; }

        /// <summary>
        ///     Gets one of the Record* classes.
        /// </summary>
        public Record Record { get; }

        /// <summary>
        ///     Gets or sets the time lived.
        /// </summary>
        public int TimeLived { get; set; }

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