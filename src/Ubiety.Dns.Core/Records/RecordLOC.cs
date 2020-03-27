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

using System.Globalization;
using System.Text;
using Ubiety.Dns.Core.Common.Extensions;

/*
 * http://www.ietf.org/rfc/rfc1876.txt
 *
2. RDATA Format

       MSB                                           LSB
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      0|        VERSION        |         SIZE          |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      2|       HORIZ PRE       |       VERT PRE        |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      4|                   LATITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      6|                   LATITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
      8|                   LONGITUDE                   |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
     10|                   LONGITUDE                   |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
     12|                   ALTITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
     14|                   ALTITUDE                    |
       +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

VERSION      Version number of the representation.  This must be zero.
             Implementations are required to check this field and make
             no assumptions about the format of unrecognized versions.

SIZE         The diameter of a sphere enclosing the described entity, in
             centimeters, expressed as a pair of four-bit unsigned
             integers, each ranging from zero to nine, with the most
             significant four bits representing the base and the second
             number representing the power of ten by which to multiply
             the base.  This allows sizes from 0e0 (<1cm) to 9e9
             (90,000km) to be expressed.  This representation was chosen
             such that the hexadecimal representation can be read by
             eye; 0x15 = 1e5.  Four-bit values greater than 9 are
             undefined, as are values with a base of zero and a non-zero
             exponent.

             Since 20000000m (represented by the value 0x29) is greater
             than the equatorial diameter of the WGS 84 ellipsoid
             (12756274m), it is therefore suitable for use as a
             "worldwide" size.

HORIZ PRE    The horizontal precision of the data, in centimeters,
             expressed using the same representation as SIZE.  This is
             the diameter of the horizontal "circle of error", rather
             than a "plus or minus" value.  (This was chosen to match
             the interpretation of SIZE; to get a "plus or minus" value,
             divide by 2.)

VERT PRE     The vertical precision of the data, in centimeters,
             expressed using the sane representation as for SIZE.  This
             is the total potential vertical error, rather than a "plus
             or minus" value.  (This was chosen to match the
             interpretation of SIZE; to get a "plus or minus" value,
             divide by 2.)  Note that if altitude above or below sea
             level is used as an approximation for altitude relative to
             the [WGS 84] ellipsoid, the precision value should be
             adjusted.

LATITUDE     The latitude of the center of the sphere described by the
             SIZE field, expressed as a 32-bit integer, most significant
             octet first (network standard byte order), in thousandths
             of a second of arc.  2^31 represents the equator; numbers
             above that are north latitude.

LONGITUDE    The longitude of the center of the sphere described by the
             SIZE field, expressed as a 32-bit integer, most significant
             octet first (network standard byte order), in thousandths
             of a second of arc, rounded away from the prime meridian.
             2^31 represents the prime meridian; numbers above that are
             east longitude.

ALTITUDE     The altitude of the center of the sphere described by the
             SIZE field, expressed as a 32-bit integer, most significant
             octet first (network standard byte order), in centimeters,
             from a base of 100,000m below the [WGS 84] reference
             spheroid used by GPS (semimajor axis a=6378137.0,
             reciprocal flattening rf=298.257223563).  Altitude above
             (or below) sea level may be used as an approximation of
             altitude relative to the the [WGS 84] spheroid, though due
             to the Earth's surface not being a perfect spheroid, there
             will be differences.  (For example, the geoid (which sea
             level approximates) for the continental US ranges from 10
             meters to 50 meters below the [WGS 84] spheroid.
             Adjustments to ALTITUDE and/or VERT PRE will be necessary
             in most cases.  The Defense Mapping Agency publishes geoid
             height values relative to the [WGS 84] ellipsoid.

 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNS location recod.
    /// </summary>
    public class RecordLoc : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordLoc" /> class.
        /// </summary>
        /// <param name="rr">Record reader of the record data.</param>
        public RecordLoc(RecordReader rr)
        {
            rr = rr.ThrowIfNull(nameof(rr));
            Version = rr.ReadByte(); // must be 0!
            Size = rr.ReadByte();
            HorizontalPrecision = rr.ReadByte();
            VerticalPrecision = rr.ReadByte();
            Latitude = rr.ReadUInt32();
            Longitude = rr.ReadUInt32();
            Altitude = rr.ReadUInt32();
        }

        /// <summary>
        ///     Gets the version of the representation.
        /// </summary>
        public byte Version { get; }

        /// <summary>
        ///     Gets the diameter of the sphere enclosing the entity.
        /// </summary>
        public byte Size { get; }

        /// <summary>
        ///     Gets the horizontal precision of the data.
        /// </summary>
        public byte HorizontalPrecision { get; }

        /// <summary>
        ///     Gets the vertical precision or the data.
        /// </summary>
        public byte VerticalPrecision { get; }

        /// <summary>
        ///     Gets the latitude of the location.
        /// </summary>
        public uint Latitude { get; }

        /// <summary>
        ///     Gets the longitude of the location.
        /// </summary>
        public uint Longitude { get; }

        /// <summary>
        ///     Gets the altitude of the location.
        /// </summary>
        public uint Altitude { get; }

        /// <summary>
        ///     Gets a string of the location.
        /// </summary>
        /// <returns>String of the location.</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1} {2} {3} {4} {5}",
                ToTime(Latitude, 'S', 'N'),
                ToTime(Longitude, 'W', 'E'),
                ToAlt(Altitude),
                SizeToString(Size),
                SizeToString(HorizontalPrecision),
                SizeToString(VerticalPrecision));
        }

        private static string ToAlt(uint a)
        {
            var alt = (a / 100.0) - 100000.00;
            return string.Format(CultureInfo.InvariantCulture, "{0:0.00}m", alt);
        }

        private static string SizeToString(byte size)
        {
            var unit = "cm";
            var prime = size >> 4;
            var power = size & 0x0f;
            if (power >= 2)
            {
                power -= 2;
                unit = "m";
            }

            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", prime);
            for (; power > 0; power--)
            {
                sb.Append('0');
            }

            sb.Append(unit);
            return sb.ToString();
        }

        private static string ToTime(uint r, char below, char above)
        {
            var mid = 2147483648; // 2^31
            char dir;
            if (r > mid)
            {
                dir = above;
                r -= mid;
            }
            else
            {
                dir = below;
                r = mid - r;
            }

            var h = r / (360000.0 * 10.0);
            var m = 60.0 * (h - (int)h);
            var s = 60.0 * (m - (int)m);
            return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2:0.000} {3}", (int)h, (int)m, s, dir);
        }
    }
}