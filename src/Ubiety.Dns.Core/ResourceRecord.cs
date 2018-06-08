using System;
using Ubiety.Dns.Core.Common;
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
    /// Resource Record (rfc1034 3.6.)
    /// </summary>
    public class ResourceRecord
    {
        private UInt32 ttl;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceRecord" /> class
        /// </summary>
        /// <param name="rr">Record reader of the record data</param>
        public ResourceRecord(RecordReader rr)
        {
            this.TimeLived = 0;
            this.Name = rr.ReadDomainName();
            this.Type = (RecordType)rr.ReadUInt16();
            this.Class = (OperationClass)rr.ReadUInt16();
            this.TTL = rr.ReadUInt32();
            this.RecordLength = rr.ReadUInt16();
            this.Record = rr.ReadRecord(this.Type);
            this.Record.ResourceRecord = this;
        }

        /// <summary>
        ///     Gets or sets the name of the node to which this resource record pertains
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///     Gets or sets the type of resource record
        /// </summary>
        public RecordType Type { get; set; }

        /// <summary>
        ///     Gets or sets the type class of resource record, mostly IN but can be CS, CH or HS
        /// </summary>
        public OperationClass Class { get; set; }

        /// <summary>
        ///     Gets or sets the time to live, the time interval that the resource record may be cached
        /// </summary>
        public UInt32 TTL
        {
            get
            {
                return (UInt32)Math.Max(0, this.ttl - this.TimeLived);
            }

            set
            {
                this.ttl = value;
            }
        }

        /// <summary>
        ///     Gets or sets the record length
        /// </summary>
        public UInt16 RecordLength { get; set; }

        /// <summary>
        ///     Gets or sets one of the Record* classes
        /// </summary>
        public Record Record { get; set; }

        /// <summary>
        ///     Gets or sets the time lived
        /// </summary>
        public Int32 TimeLived { get; set; }

        /// <summary>
        ///     String version of the resource record
        /// </summary>
        /// <returns>String of the resource</returns>
        public override String ToString()
        {
            return $"{this.Name, -32} {this.TTL}\t{this.Class}\t{this.Type}\t{this.Record}";
        }
    }
}
