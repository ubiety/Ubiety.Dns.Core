using System;

namespace Ubiety.Dns.Core.Records.NotUsed
{
    /// <summary>
    ///     DNS public key resource record
    /// </summary>
    public class RecordDNSKey : Record
    {
        /// <summary>
        ///     Gets or sets the record data
        /// </summary>
        public byte[] RecordData { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordDNSKey" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordDNSKey(RecordReader rr)
        {
            // re-read length
            ushort length = rr.ReadUInt16(-2);
            this.RecordData = rr.ReadBytes(length);
        }

        /// <summary>
        ///     String representation of the record
        /// </summary>
        /// <returns>String of the data</returns>
        public override string ToString()
        {
            return "not-used";
        }
    }
}
