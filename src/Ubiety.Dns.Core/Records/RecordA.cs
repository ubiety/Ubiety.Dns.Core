using System;
using System.Net;
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
        private IPAddress address;

        /// <summary>
        /// </summary>
        public IPAddress Address { get => address; set => address = value; }

        /// <summary>
        /// </summary>
        public RecordA(RecordReader rr)
        {
            IPAddress.TryParse(
                string.Format("{0}.{1}.{2}.{3}",
                rr.ReadByte(),
                rr.ReadByte(),
                rr.ReadByte(),
                rr.ReadByte()), out this.address);
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return this.Address.ToString();
        }

    }
}
