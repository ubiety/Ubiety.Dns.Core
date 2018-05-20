using System;
using System.Globalization;

namespace Ubiety.Dns.Core.Records
{
    /*
    3.3.9. MX RDATA format

        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        |                  PREFERENCE                   |
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
        /                   EXCHANGE                    /
        /                                               /
        +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

    where:

    PREFERENCE      A 16 bit integer which specifies the preference given to
                    this RR among others at the same owner.  Lower values
                    are preferred.

    EXCHANGE        A <domain-name> which specifies a host willing to act as
                    a mail exchange for the owner name.

    MX records cause type A additional section processing for the host
    specified by EXCHANGE.  The use of MX RRs is explained in detail in
    [RFC-974].
    */

    /// <summary>
    ///     Mail exchange DNS record
    /// </summary>
    public class RecordMx : Record, IComparable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMx" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordMx(RecordReader rr)
        {
            this.Preference = rr.ReadUInt16();
            this.Exchange = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the preference
        /// </summary>
        public UInt16 Preference { get; set; }

        /// <summary>
        ///     Gets or sets the exchange
        /// </summary>
        public String Exchange { get; set; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>Exchange and preference as a string</returns>
        public override String ToString()
        {
            return $"{this.Preference} {this.Exchange}";
        }

        /// <summary>
        ///     Compares record to an object
        /// </summary>
        /// <param name="obj">Object to compare record to</param>
        /// <returns>Int value of the comparison</returns>
        public int CompareTo(object obj)
        {
            RecordMx recordMX = obj as RecordMx;
            if (recordMX == null)
            {
                return -1;
            }
            else if (this.Preference > recordMX.Preference)
            {
                return 1;
            }
            else if (this.Preference < recordMX.Preference)
            {
                return -1;
            }
            else
            {
                // they are the same, now compare case insensitive names
                return string.Compare(this.Exchange, recordMX.Exchange, true, CultureInfo.InvariantCulture);
            }
        }
    }
}
