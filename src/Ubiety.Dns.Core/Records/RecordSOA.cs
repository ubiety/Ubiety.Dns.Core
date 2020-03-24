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
3.3.13. SOA RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     MNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                     RNAME                     /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    SERIAL                     |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    REFRESH                    |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                     RETRY                     |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    EXPIRE                     |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    |                    MINIMUM                    |
    |                                               |
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MNAME           The <domain-name> of the name server that was the
                original or primary source of data for this zone.

RNAME           A <domain-name> which specifies the mailbox of the
                person responsible for this zone.

SERIAL          The unsigned 32 bit version number of the original copy
                of the zone.  Zone transfers preserve this value.  This
                value wraps and should be compared using sequence space
                arithmetic.

REFRESH         A 32 bit time interval before the zone should be
                refreshed.

RETRY           A 32 bit time interval that should elapse before a
                failed refresh should be retried.

EXPIRE          A 32 bit time value that specifies the upper limit on
                the time interval that can elapse before the zone is no
                longer authoritative.

MINIMUM         The unsigned 32 bit minimum TTL field that should be
                exported with any RR from this zone.

SOA records cause no additional section processing.

All times are in units of seconds.

Most of these fields are pertinent only for name server maintenance
operations.  However, MINIMUM is used in all query operations that
retrieve RRs from a zone.  Whenever a RR is sent in a response to a
query, the TTL field is set to the maximum of the TTL field from the RR
and the MINIMUM field in the appropriate SOA.  Thus MINIMUM is a lower
bound on the TTL field for all RRs in a zone.  Note that this use of
MINIMUM should occur when the RRs are copied into the response and not
when the zone is loaded from a master file or via a zone transfer.  The
reason for this provison is to allow future dynamic update facilities to
change the SOA RR with known semantics.
*/

using Ubiety.Dns.Core.Common;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNS Start of Authority record.
    /// </summary>
    public class RecordSoa : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordSoa" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordSoa(RecordReader rr)
        {
            rr = rr.ThrowIfNull(nameof(rr));
            PrimaryNameserver = rr.ReadDomainName();
            ResponsibleDomain = rr.ReadDomainName();
            Serial = rr.ReadUInt32();
            Refresh = rr.ReadUInt32();
            Retry = rr.ReadUInt32();
            Expire = rr.ReadUInt32();
            Minimum = rr.ReadUInt32();
        }

        /// <summary>
        ///     Gets or sets the primary nameserver.
        /// </summary>
        public string PrimaryNameserver { get; set; }

        /// <summary>
        ///     Gets or sets the responsible domain.
        /// </summary>
        public string ResponsibleDomain { get; set; }

        /// <summary>
        ///     Gets or sets the serial.
        /// </summary>
        public uint Serial { get; set; }

        /// <summary>
        ///     Gets or sets the refresh interval.
        /// </summary>
        public uint Refresh { get; set; }

        /// <summary>
        ///     Gets or sets the retry interval.
        /// </summary>
        public uint Retry { get; set; }

        /// <summary>
        ///     Gets or sets the expiration time.
        /// </summary>
        public uint Expire { get; set; }

        /// <summary>
        ///     Gets or sets the minimum TTL.
        /// </summary>
        public uint Minimum { get; set; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Record data as the string.</returns>
        public override string ToString()
        {
            return $"{PrimaryNameserver} {ResponsibleDomain} {Serial} {Refresh} {Retry} {Expire} {Minimum}";
        }
    }
}