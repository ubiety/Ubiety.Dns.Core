using System;
using System.Globalization;

/*
 * http://tools.ietf.org/rfc/rfc2230.txt
 *
 * 3.1 KX RDATA format

   The KX DNS record has the following RDATA format:

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                  PREFERENCE                   |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   EXCHANGER                   /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

   where:

   PREFERENCE      A 16 bit non-negative integer which specifies the
                   preference given to this RR among other KX records
                   at the same owner.  Lower values are preferred.

   EXCHANGER       A <domain-name> which specifies a host willing to
                   act as a mail exchange for the owner name.

   KX records MUST cause type A additional section processing for the
   host specified by EXCHANGER.  In the event that the host processing
   the DNS transaction supports IPv6, KX records MUST also cause type
   AAAA additional section processing.

   The KX RDATA field MUST NOT be compressed.

 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Key exchange record
    /// </summary>
    public class RecordKx : Record, IComparable, IEquatable<RecordKx>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordKx" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the data</param>
        public RecordKx(RecordReader rr)
        {
            this.Preference = rr.ReadUInt16();
            this.Exchanger = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the preference
        /// </summary>
        public UInt16 Preference { get; set; }

        /// <summary>
        ///     Gets or sets the exchanger
        /// </summary>
        public String Exchanger { get; set; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>String version of the record</returns>
        public override String ToString()
        {
            return $"{this.Preference} {this.Exchanger}";
        }

        /// <summary>
        ///     Compares instance to an object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Integer representing the comparison</returns>
        public Int32 CompareTo(Object obj)
        {
            RecordKx recordKX = obj as RecordKx;
            if (recordKX == null)
            {
                return -1;
            }
            else if (this.Preference > recordKX.Preference)
            {
                return 1;
            }
            else if (this.Preference < recordKX.Preference)
            {
                return -1;
            }
            else
            {
                // they are the same, now compare case insensitive names
                return String.Compare(this.Exchanger, recordKX.Exchanger, true, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        ///     Overrides equals
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating whether the instances are equal</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as RecordKx);
        }

        /// <summary>
        ///     Are two instances of <see cref="RecordKx" /> equal
        /// </summary>
        /// <param name="other"><see cref="RecordKx" /> to compare to</param>
        /// <returns>Boolean indicating whether the two instances are equal</returns>
        public Boolean Equals(RecordKx other)
        {
            if (other == null)
            {
                return false;
            }

            return String.Equals(this.Exchanger, other.Exchanger);
        }

        /// <summary>
        ///     Gets the hash code
        /// </summary>
        /// <returns>Integer value of the hash</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hashcode = 13;
                hashcode = (hashcode * 397) ^ this.Preference;
                var exHash = !String.IsNullOrEmpty(this.Exchanger) ? this.Exchanger.GetHashCode() : 0;
                hashcode = (hashcode * 397) ^ exHash;
                return hashcode;
            }
        }
    }
}
