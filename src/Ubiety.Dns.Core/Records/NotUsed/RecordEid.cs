using System;
using System.Collections.ObjectModel;

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     DNS entity identifier resource record
    /// </summary>
    public class RecordEid : Record
    {
        private readonly Collection<Byte> data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordEid" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordEid(RecordReader rr)
        {
            // re-read length
            UInt16 length = rr.ReadUInt16(-2);
            this.data = new Collection<Byte>(rr.ReadBytes(length));
        }

        /// <summary>
        ///     Gets the record data
        /// </summary>
        public Collection<Byte> RecordData { get => this.data; }

        /// <summary>
        ///     String version of the resource record
        /// </summary>
        /// <returns>String of the record data</returns>
        public override String ToString()
        {
            return "not-used";
        }
    }
}
