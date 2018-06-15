using System;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    /// DNS AFSDB Record
    /// </summary>
    /// <remarks>
    /// # [Description](#tab/description)
    ///
    /// AFS Database location resource record
    ///
    /// # [RFC](#tab/rfc)
    ///
    /// This section defines an extension of the DNS to locate servers both
    /// for AFS (AFS is a registered trademark of Transarc Corporation) and
    /// for the Open Source Foundation's (OSF) Distributed Computing
    /// Environment (DCE) authenticated naming system using HP/Apollo's NCA,
    /// both to be components of the OSF DCE. The discussion assumes that
    /// the reader is familiar with AFS [5] and NCA [6].
    ///
    /// The AFS (originally the Andrew File System) system uses the DNS to
    /// map from a domain name to the name of an AFS cell database server.
    /// The DCE Naming service uses the DNS for a similar function: mapping
    /// from the domain name of a cell to authenticated name servers for that
    /// cell. The method uses a new RR type with mnemonic AFSDB and type
    /// code of 18 (decimal).
    ///
    /// AFSDB has the following format:
    ///
    /// [owner] [ttl] [class] AFSDB [subtype] [hostname]
    ///
    /// Both RDATA fields are required in all AFSDB RRs. The [subtype] field
    /// is a 16 bit integer. The [hostname] field is a domain name of a host
    /// that has a server for the cell named by the owner name of the RR
    /// </remarks>
    public class RecordAfsdb : Record
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordAfsdb" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordAfsdb(RecordReader rr)
        {
            this.SubType = rr.ReadUInt16();
            this.Hostname = rr.ReadDomainName();
        }

        /// <summary>
        /// Gets the subtype
        /// </summary>
        /// <value>AFSDB subtype as an unsigned short</value>
        public UInt16 SubType { get; }

        /// <summary>
        /// Gets the hostname
        /// </summary>
        /// <value>AFSDB hostname as a string</value>
        public String Hostname { get; }

        /// <summary>
        ///     String representation of the record
        /// </summary>
        /// <returns>String version of the data</returns>
        public override String ToString()
        {
            return $"{this.SubType}{this.Hostname}";
        }
    }
}
