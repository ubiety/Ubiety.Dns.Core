/* 
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *  
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *  
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *  
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Globalization;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mail exchange DNS record.
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
    ///     ```.
    /// </remarks>
    public sealed class RecordMx : Record, IComparable, IEquatable<RecordMx>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMx" /> class.
        /// </summary>
        /// <param name="reader"><see cref="RecordReader" /> for the record data.</param>
        public RecordMx(RecordReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            Preference = reader.ReadUInt16();
            Exchange = reader.ReadDomainName();
        }

        /// <summary>
        ///     Gets the preference.
        /// </summary>
        public ushort Preference { get; }

        /// <summary>
        ///     Gets the exchange.
        /// </summary>
        public string Exchange { get; }

        /// <summary>
        /// Is the left less than the right.
        /// </summary>
        /// <param name="x">Left comparison object.</param>
        /// <param name="y">Right comparison object.</param>
        public static bool operator <(RecordMx x, RecordMx y)
        {
            return CompareTo(x.ThrowIfNull(nameof(x)), y.ThrowIfNull(nameof(y))) < 0;
        }

        /// <summary>
        /// Is the left greater than the right.
        /// </summary>
        /// <param name="x">Left comparison object.</param>
        /// <param name="y">Right comparison object.</param>
        public static bool operator >(RecordMx x, RecordMx y)
        {
            return CompareTo(x.ThrowIfNull(nameof(x)), y) > 0;
        }

        /// <summary>
        /// Is the left less than or equal to the right.
        /// </summary>
        /// <param name="x">Left comparison object.</param>
        /// <param name="y">Right comparison object.</param>
        public static bool operator <=(RecordMx x, RecordMx y)
        {
            return CompareTo(x.ThrowIfNull(nameof(x)), y) <= 0;
        }

        /// <summary>
        /// Is the left greater than or equal to the right.
        /// </summary>
        /// <param name="x">Left comparison object.</param>
        /// <param name="y">Right comparison object.</param>
        public static bool operator >=(RecordMx x, RecordMx y)
        {
            return CompareTo(x.ThrowIfNull(nameof(x)), y) >= 0;
        }

        /// <summary>
        /// Are the left and right objects equal.
        /// </summary>
        /// <param name="x">Left comparison object.</param>
        /// <param name="y">Right comparison object.</param>
        public static bool operator ==(RecordMx x, RecordMx y)
        {
            return CompareTo(x.ThrowIfNull(nameof(x)), y) == 0;
        }

        /// <summary>
        /// Are the left and right objects not equal.
        /// </summary>
        /// <param name="x">Left comparison object.</param>
        /// <param name="y">Right comparison object.</param>
        public static bool operator !=(RecordMx x, RecordMx y)
        {
            return CompareTo(x.ThrowIfNull(nameof(x)), y) != 0;
        }

        /// <summary>
        ///     Compares record to an object.
        /// </summary>
        /// <param name="obj">Object to compare record to.</param>
        /// <returns>Int value of the comparison.</returns>
        public int CompareTo(object obj)
        {
            return CompareTo(this, obj as RecordMx);
        }

        /// <summary>
        ///     Does this instance equal another instance of RecordMx.
        /// </summary>
        /// <param name="other">RecordMx to compare to.</param>
        /// <returns>Boolean indicating whether the objects are equal.</returns>
        public bool Equals(RecordMx other)
        {
            if (other is null)
            {
                return false;
            }

            return CompareTo(this, other) == 0;
        }

        /// <summary>
        ///     Does this instance equal an object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Boolean indicating whether the objects are equal.</returns>
        public override bool Equals(object obj)
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
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Exchange and preference as a string.</returns>
        public override string ToString()
        {
            return $"{Preference} {Exchange}";
        }

        /// <summary>
        ///     Gets the record hash code.
        /// </summary>
        /// <returns>Integer representing the hash code.</returns>
        public override int GetHashCode()
        {
            return Exchange.GetHashCode();
        }

        private static int CompareTo(RecordMx x, RecordMx y)
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