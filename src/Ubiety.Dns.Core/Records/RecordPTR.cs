/*
 3.3.12. PTR RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   PTRDNAME                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

PTRDNAME        A <domain-name> which points to some location in the
                domain name space.

PTR records cause no additional section processing.  These RRs are used
in special domains to point to some other location in the domain space.
These records are simple data, and don't imply any special processing
similar to that performed by CNAME, which identifies aliases.  See the
description of the IN-ADDR.ARPA domain for an example.
 */

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Pointer DNS record
    /// </summary>
    public class RecordPtr : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordPtr" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordPtr(RecordReader rr)
        {
            PointerDomain = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the pointer domain
        /// </summary>
        public string PointerDomain { get; set; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>Pointer domain as a string</returns>
        public override string ToString()
        {
            return PointerDomain;
        }
    }
}