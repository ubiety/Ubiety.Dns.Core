using System;
using System.Collections.ObjectModel;

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Asynchronous transfer mode address resource record
    /// </summary>
    public class RecordAtma : Record
    {
        private Collection<byte> data;
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAtma" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordAtma(RecordReader rr)
        {
            // re-read length
            ushort length = rr.ReadUInt16(-2);
            this.data = new Collection<byte>(rr.ReadBytes(length));
        }

        /// <summary>
        ///     Gets or sets the record data
        /// </summary>
        public Collection<byte> RecordData { get => this.data; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>String version of the record</returns>
        public override string ToString()
        {
            return "not-used";
        }
    }
}
