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

/*
 *  http://www.ietf.org/rfc/rfc2782.txt
 *
   Priority
        The priority of this target host.  A client MUST attempt to
        contact the target host with the lowest-numbered priority it can
        reach; target hosts with the same priority SHOULD be tried in an
        order defined by the weight field.  The range is 0-65535.  This
        is a 16 bit unsigned integer in network byte order.

   Weight
        A server selection mechanism.  The weight field specifies a
        relative weight for entries with the same priority. Larger
        weights SHOULD be given a proportionately higher probability of
        being selected. The range of this number is 0-65535.  This is a
        16 bit unsigned integer in network byte order.  Domain
        administrators SHOULD use Weight 0 when there isn't any server
        selection to do, to make the RR easier to read for humans (less
        noisy).  In the presence of records containing weights greater
        than 0, records with weight 0 should have a very small chance of
        being selected.

        In the absence of a protocol whose specification calls for the
        use of other weighting information, a client arranges the SRV
        RRs of the same Priority in the order in which target hosts,
        specified by the SRV RRs, will be contacted. The following
        algorithm SHOULD be used to order the SRV RRs of the same
        priority:

        To select a target to be contacted next, arrange all SRV RRs
        (that have not been ordered yet) in any order, except that all
        those with weight 0 are placed at the beginning of the list.

        Compute the sum of the weights of those RRs, and with each RR
        associate the running sum in the selected order. Then choose a
        uniform random number between 0 and the sum computed
        (inclusive), and select the RR whose running sum value is the
        first in the selected order which is greater than or equal to
        the random number selected. The target host specified in the
        selected SRV RR is the next one to be contacted by the client.
        Remove this SRV RR from the set of the unordered SRV RRs and
        apply the described algorithm to the unordered SRV RRs to select
        the next target host.  Continue the ordering process until there
        are no unordered SRV RRs.  This process is repeated for each
        Priority.

   Port
        The port on this target host of this service.  The range is 0-
        65535.  This is a 16 bit unsigned integer in network byte order.
        This is often as specified in Assigned Numbers but need not be.

   Target
        The domain name of the target host.  There MUST be one or more
        address records for this name, the name MUST NOT be an alias (in
        the sense of RFC 1034 or RFC 2181).  Implementors are urged, but
        not required, to return the address record(s) in the Additional
        Data section.  Unless and until permitted by future standards
        action, name compression is not to be used for this field.

        A Target of "." means that the service is decidedly not
        available at this domain.

 */

using System;
using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Service DNS record.
    /// </summary>
    public class RecordSrv : Record, IComparable<RecordSrv>, IEquatable<RecordSrv>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSrv" /> class.
        /// </summary>
        /// <param name="recordReader"><see cref="RecordReader" /> of the record data.</param>
        public RecordSrv(RecordReader recordReader)
        {
            recordReader = recordReader.ThrowIfNull(nameof(recordReader));
            Priority = recordReader.ReadUInt16();
            Weight = recordReader.ReadUInt16();
            Port = recordReader.ReadUInt16();
            Target = recordReader.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the record priority.
        /// </summary>
        public ushort Priority { get; set; }

        /// <summary>
        ///     Gets or sets the record weight.
        /// </summary>
        public ushort Weight { get; set; }

        /// <summary>
        ///     Gets or sets the service port.
        /// </summary>
        public ushort Port { get; set; }

        /// <summary>
        ///     Gets or sets the target domain.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        ///     Determines if the record is greater than another.
        /// </summary>
        /// <param name="left">Left record.</param>
        /// <param name="right">Right record.</param>
        /// <returns>A value indicating whether the left record is greater.</returns>
        public static bool operator >(RecordSrv left, RecordSrv right)
        {
            return left.ThrowIfNull(nameof(left)).CompareTo(right) == 1;
        }

        /// <inheritdoc cref="IComparable{T}" />
        public static bool operator <(RecordSrv left, RecordSrv right)
        {
            return left.ThrowIfNull(nameof(left)).CompareTo(right) == -1;
        }

        /// <inheritdoc cref="IComparable{T}" />
        public static bool operator <=(RecordSrv left, RecordSrv right)
        {
            return left.ThrowIfNull(nameof(left)).CompareTo(right) <= 0;
        }

        /// <inheritdoc cref="IComparable{T}" />
        public static bool operator >=(RecordSrv left, RecordSrv right)
        {
            return left.ThrowIfNull(nameof(left)).CompareTo(right) >= 0;
        }

        /// <inheritdoc cref="IEquatable{T}" />
        public static bool operator ==(RecordSrv left, RecordSrv right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        /// <inheritdoc cref="IEquatable{T}" />
        public static bool operator !=(RecordSrv left, RecordSrv right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Compares instance to object.
        /// </summary>
        /// <param name="other">Object to compare to.</param>
        /// <returns>Integer defining object order.</returns>
        public int CompareTo(RecordSrv other)
        {
            if (other is null)
            {
                return 1;
            }

            if (Priority.CompareTo(other.Priority) > 0)
            {
                return 1;
            }
            else
            {
                return Weight.CompareTo(other.Weight);
            }
        }

        /// <summary>
        ///     Compares two instances for equality.
        /// </summary>
        /// <param name="other">Second instance to compare.</param>
        /// <returns>Value indicating whether the values are equal.</returns>
        public bool Equals(RecordSrv other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Record as a string.</returns>
        public override string ToString()
        {
            return $"{Priority} {Weight} {Port} {Target}";
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}