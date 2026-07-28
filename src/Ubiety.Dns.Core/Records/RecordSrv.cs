/*
 * Copyright 2020 Dieter Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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

namespace Ubiety.Dns.Core.Records;

/// <summary>
///     RFC 2782 - DNS resource record for service discovery.
/// </summary>
public sealed record RecordSrv : Record, IComparable<RecordSrv>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RecordSrv"/> class from the specified <see cref="RecordReader"/>.
    /// </summary>
    /// <param name="reader">The <see cref="RecordReader"/> used to read the SRV record data.</param>
    public RecordSrv(RecordReader reader)
        : base(reader)
    {
        Priority = Reader.ReadUInt16();
        Weight = Reader.ReadUInt16();
        Port = Reader.ReadUInt16();
        Target = Reader.ReadDomainName();
    }

    /// <summary>
    ///     Gets the priority of the target host. Lower values are tried first.
    /// </summary>
    public ushort Priority { get; }

    /// <summary>
    ///     Gets the relative weight for entries with the same priority. Higher values are more likely to be selected.
    /// </summary>
    public ushort Weight { get; }

    /// <summary>
    ///     Gets the port on this target host of this service.
    /// </summary>
    public ushort Port { get; }

    /// <summary>
    ///     Gets the domain name of the target host for this service.
    /// </summary>
    public string Target { get; }

    /// <summary>
    ///     Determines if the left record is greater than the right record based on priority and weight.
    /// </summary>
    /// <param name="left">The left <see cref="RecordSrv"/> instance.</param>
    /// <param name="right">The right <see cref="RecordSrv"/> instance.</param>
    /// <returns><c>true</c> if the left record is greater; otherwise, <c>false</c>.</returns>
    public static bool operator >(RecordSrv left, RecordSrv right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) == 1;
    }

    /// <inheritdoc cref="IComparable{T}" />
    public static bool operator <(RecordSrv left, RecordSrv right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) == -1;
    }

    /// <inheritdoc cref="IComparable{T}" />
    public static bool operator <=(RecordSrv left, RecordSrv right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) <= 0;
    }

    /// <inheritdoc cref="IComparable{T}" />
    public static bool operator >=(RecordSrv left, RecordSrv right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    ///     Returns a string representation of the SRV record data.
    /// </summary>
    /// <returns>A string containing the SRV record fields in display order.</returns>
    public override string ToString()
    {
        return $"{Priority} {Weight} {Port} {Target}";
    }

    /// <summary>
    ///     Compares this instance to another <see cref="RecordSrv"/> instance for ordering.
    /// </summary>
    /// <param name="other">The <see cref="RecordSrv"/> instance to compare to, which may be null.</param>
    /// <returns>An integer that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(RecordSrv? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Priority.CompareTo(other.Priority) > 0 ? 1 : Weight.CompareTo(other.Weight);
    }
}
