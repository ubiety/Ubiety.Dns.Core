using System;
using System.Globalization;

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mail exchange DNS record
    /// </summary>
    /// <remarks>
    ///     # [Description](#tab/description)
    ///     Standard MX mail DNS record
    ///     # [RFC](#tab/rfc)
    ///     ```
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     |                  PREFERENCE                   |
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     /                   EXCHANGE                    /
    ///     /                                               /
    ///     +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    ///     where:
    ///     PREFERENCE      A 16 bit integer which specifies the preference given to
    ///     this RR among others at the same owner.  Lower values
    ///     are preferred.
    ///     EXCHANGE        A [domain-name] which specifies a host willing to act as
    ///     a mail exchange for the owner name.
    ///     MX records cause type A additional section processing for the host
    ///     specified by EXCHANGE.  The use of MX RRs is explained in detail in
    ///     [RFC-974].
    ///     ```
    /// </remarks>
    public sealed class RecordMx : Record, IComparable, IEquatable<RecordMx>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMx" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordMx(RecordReader rr)
        {
            Preference = rr.ReadUInt16();
            Exchange = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets the preference
        /// </summary>
        public UInt16 Preference { get; }

        /// <summary>
        ///     Gets the exchange
        /// </summary>
        public String Exchange { get; }

        /// <summary>
        /// Is the left less than the right
        /// </summary>
        /// <param name="x">Left comparison object</param>
        /// <param name="y">Right comparison object</param>
        public static Boolean operator <(RecordMx x, RecordMx y)
        {
            return CompareTo(x, y) < 0;
        }

        /// <summary>
        /// Is the left greater than the right
        /// </summary>
        /// <param name="x">Left comparison object</param>
        /// <param name="y">Right comparison object</param>
        public static Boolean operator >(RecordMx x, RecordMx y)
        {
            return CompareTo(x, y) > 0;
        }

        /// <summary>
        /// Is the left less than or equal to the right
        /// </summary>
        /// <param name="x">Lefyt comparison object</param>
        /// <param name="y">Right comparison object</param>
        public static Boolean operator <=(RecordMx x, RecordMx y)
        {
            return CompareTo(x, y) <= 0;
        }

        /// <summary>
        /// Is the left greater than or equal to the right
        /// </summary>
        /// <param name="x">Left comparison object</param>
        /// <param name="y">Right comparison object</param>
        public static Boolean operator >=(RecordMx x, RecordMx y)
        {
            return CompareTo(x, y) >= 0;
        }

        /// <summary>
        /// Are the left and right objects equal
        /// </summary>
        /// <param name="x">Left comparison object</param>
        /// <param name="y">Right comparison object</param>
        public static Boolean operator ==(RecordMx x, RecordMx y)
        {
            return CompareTo(x, y) == 0;
        }

        /// <summary>
        /// Are the left and right objects not equal
        /// </summary>
        /// <param name="x">Left comparison object</param>
        /// <param name="y">Right comparison object</param>
        public static Boolean operator !=(RecordMx x, RecordMx y)
        {
            return CompareTo(x, y) != 0;
        }

        /// <summary>
        ///     Compares record to an object
        /// </summary>
        /// <param name="obj">Object to compare record to</param>
        /// <returns>Int value of the comparison</returns>
        public Int32 CompareTo(Object obj)
        {
            return CompareTo(this, obj as RecordMx);
        }

        /// <summary>
        ///     Does this instance equal another instance of RecordMx
        /// </summary>
        /// <param name="other">RecordMx to compare to</param>
        /// <returns>Boolean indicating whether the objects are equal</returns>
        public Boolean Equals(RecordMx other)
        {
            if (other is null)
            {
                return false;
            }

            return CompareTo(this, other) == 0;
        }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>Exchange and preference as a string</returns>
        public override String ToString()
        {
            return $"{Preference} {Exchange}";
        }

        /// <summary>
        ///     Does this instance equal an object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Boolean indicating whether the objects are equal</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return ReferenceEquals(this, obj) || Equals(obj as RecordMx);
        }

        /// <summary>
        ///     Gets the record hash code
        /// </summary>
        /// <returns>Integer representing the hash code</returns>
        public override Int32 GetHashCode()
        {
            return Exchange.GetHashCode();
        }

        private static Int32 CompareTo(RecordMx x, RecordMx y)
        {
            if (y == null)
            {
                return -1;
            }

            if (x.Preference > y.Preference)
            {
                return 1;
            }

            if (x.Preference < y.Preference)
            {
                return -1;
            }

            return string.Compare(x.Exchange, y.Exchange, true, CultureInfo.InvariantCulture);
        }
    }
}