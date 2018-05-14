using System;
using System.Collections.ObjectModel;

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     Sender policy framework
    /// </summary>
    public class RecordSPF : Record
    {
        private Collection<byte> data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSPF" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordSPF(RecordReader rr)
        {
            // re-read length
            ushort length = rr.ReadUInt16(-2);
            this.RecordData = new Collection<byte>(rr.ReadBytes(length));
        }

        /// <summary>
        ///     Gets the record data
        /// </summary>
        public Collection<byte> RecordData { get; set; }

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
