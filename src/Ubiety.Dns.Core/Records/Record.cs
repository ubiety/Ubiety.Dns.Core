using System;
using System.Collections.ObjectModel;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Abstract record
    /// </summary>
    public abstract class Record
    {
        /// <summary>
        ///     Record data
        /// </summary>
        protected readonly Collection<Byte> data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Record" /> class
        /// </summary>
        public Record()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Record" /> class
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data</param>
        public Record(RecordReader reader)
        {
            UInt16 length = reader.ReadUInt16(-2);
            this.data = new Collection<Byte>(reader.ReadBytes(length));
        }

        /// <summary>
        ///     Gets the record data
        /// </summary>
        public Collection<Byte> RecordData { get => this.data; }

        /// <summary>
        ///     Gets or sets the resource record this record is a part of
        /// </summary>
        public ResourceRecord ResourceRecord { get; set; }

        /// <summary>
        ///     String representation of the record
        /// </summary>
        /// <returns>String version of the data</returns>
        public override String ToString()
        {
            return "not-used";
        }
    }
}
