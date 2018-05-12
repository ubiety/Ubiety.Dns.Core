using System;

namespace Heijden.DNS
{
        /// <summary>
        /// </summary>
    public class RecordUnknown : Record
    {
        /// <summary>
        /// </summary>
        public byte[] RDATA;
        /// <summary>
        /// </summary>
        public RecordUnknown(RecordReader rr)
        {
            // re-read length
            ushort RDLENGTH = rr.ReadUInt16(-2);
            RDATA = rr.ReadBytes(RDLENGTH);
        }
    }
}
