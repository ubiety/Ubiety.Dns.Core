using System;
using System.Collections.ObjectModel;

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Sender policy framework
    /// </summary>
    public class RecordSpf : Record
    {
        private Collection<byte> data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSpf" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordSpf(RecordReader rr)
        {
            // re-read length
            ushort length = rr.ReadUInt16(-2);
            this.data = new Collection<byte>(rr.ReadBytes(length));
        }

        /// <summary>
        ///     Gets the record data
        /// </summary>
        public Collection<byte> RecordData { get => this.data; }

        /// <summary>
        ///     String version of the record
        /// </summary>
        /// <returns>String of the record</returns>
        public override string ToString()
        {
            return "not-used";
        }
    }
}
