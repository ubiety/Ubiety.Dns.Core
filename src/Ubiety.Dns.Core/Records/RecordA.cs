using System;
/*
 3.4.1. A RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    ADDRESS                    |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

ADDRESS         A 32 bit Internet address.

Hosts that have multiple Internet addresses will have multiple A
records.
 * 
 */
namespace Ubiety.Dns.Core.Records
{
        /// <summary>
        /// </summary>
    public class RecordA : Record
    {
        /// <summary>
        /// </summary>
        public System.Net.IPAddress Address;

        /// <summary>
        /// </summary>
        public RecordA(RecordReader rr)
        {
            System.Net.IPAddress.TryParse(string.Format("{0}.{1}.{2}.{3}",
                rr.ReadByte(),
                rr.ReadByte(),
                rr.ReadByte(),
                rr.ReadByte()), out this.Address);
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return Address.ToString();
        }

    }
}
