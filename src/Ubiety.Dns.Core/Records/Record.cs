using System;
using System.Collections.Generic;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    /// Abstract record
    /// </summary>
    public abstract class Record
    {
        /// <summary>
        /// Record data
        /// </summary>
        private readonly List<Byte> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Record" /> class.
        /// </summary>
        protected Record()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Record" /> class
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data</param>
        protected Record(RecordReader reader)
        {
            UInt16 length = reader.ReadUInt16(-2);
            this.data = new List<Byte>(reader.ReadBytes(length));
        }

        /// <summary>
        /// Gets the record data
        /// </summary>
        /// <value>Byte list of the raw record data</value>
        public List<Byte> RecordData { get => this.data; }

        /// <summary>
        /// Gets or sets the resource record this record is a part of
        /// </summary>
        /// <value>Resource record of the data</value>
        public ResourceRecord ResourceRecord { get; set; }

        /// <summary>
        /// String representation of the record
        /// </summary>
        /// <returns>String version of the data</returns>
        public override String ToString()
        {
            return $"{this.GetType().Name} is not-used";
        }
    }
}
