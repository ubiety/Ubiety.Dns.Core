/* http://tools.ietf.org/rfc/rfc1183.txt

3.1. The X25 RR

   The X25 RR is defined with mnemonic X25 and type code 19 (decimal).

   X25 has the following format:

   <owner> <ttl> <class> X25 <PSDN-address>

   <PSDN-address> is required in all X25 RRs.

   <PSDN-address> identifies the PSDN (Public Switched Data Network)
   address in the X.121 [10] numbering plan associated with <owner>.
   Its format in master files is a <character-string> syntactically
   identical to that used in TXT and HINFO.

   The format of X25 is class insensitive.  X25 RRs cause no additional
   section processing.

   The <PSDN-address> is a string of decimal digits, beginning with the
   4 digit DNIC (Data Network Identification Code), as specified in
   X.121. National prefixes (such as a 0) MUST NOT be used.

   For example:

   Relay.Prime.COM.  X25       311061700956


 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     X25 DNS record
    /// </summary>
    public class RecordX25 : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordX25" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordX25(RecordReader rr)
        {
            PSDNAdress = rr.ReadString();
        }

        /// <summary>
        ///     Gets or sets the PSDN address
        /// </summary>
        public string PSDNAdress { get; set; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>PSDN address as a string</returns>
        public override string ToString()
        {
            return PSDNAdress;
        }
    }
}