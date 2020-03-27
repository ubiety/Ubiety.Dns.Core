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

/* http://tools.ietf.org/rfc/rfc1183.txt

3.3. The Route Through RR

   The Route Through RR is defined with mnemonic RT and type code 21
   (decimal).

   The RT resource record provides a route-through binding for hosts
   that do not have their own direct wide area network addresses.  It is
   used in much the same way as the MX RR.

   RT has the following format:

   <owner> <ttl> <class> RT <preference> <intermediate-host>

   Both RDATA fields are required in all RT RRs.

   The first field, <preference>, is a 16 bit integer, representing the
   preference of the route.  Smaller numbers indicate more preferred
   routes.

   <intermediate-host> is the domain name of a host which will serve as
   an intermediate in reaching the host specified by <owner>.  The DNS
   RRs associated with <intermediate-host> are expected to include at
   least one A, X25, or ISDN record.

   The format of the RT RR is class insensitive.  RT records cause type
   X25, ISDN, and A additional section processing for <intermediate-
   host>.

   For example,

   sh.prime.com.      IN   RT   2    Relay.Prime.COM.
                      IN   RT   10   NET.Prime.COM.
   *.prime.com.       IN   RT   90   Relay.Prime.COM.

   When a host is looking up DNS records to attempt to route a datagram,
   it first looks for RT records for the destination host, which point
   to hosts with address records (A, X25, ISDN) compatible with the wide
   area networks available to the host.  If it is itself in the set of
   RT records, it discards any RTs with preferences higher or equal to
   its own.  If there are no (remaining) RTs, it can then use address
   records of the destination itself.

   Wild-card RTs are used exactly as are wild-card MXs.  RT's do not
   "chain"; that is, it is not valid to use the RT RRs found for a host
   referred to by an RT.

   The concrete encoding is identical to the MX RR.


 */

using Ubiety.Dns.Core.Common.Extensions;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Route through DNS record.
    /// </summary>
    public class RecordRt : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordRt" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordRt(RecordReader rr)
        {
            rr = rr.ThrowIfNull(nameof(rr));
            Preference = rr.ReadUInt16();
            IntermediateHost = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the route preference.
        /// </summary>
        public ushort Preference { get; set; }

        /// <summary>
        ///     Gets or sets the intermediate host.
        /// </summary>
        public string IntermediateHost { get; set; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>Preference and host as a string.</returns>
        public override string ToString()
        {
            return $"{Preference} {IntermediateHost}";
        }
    }
}