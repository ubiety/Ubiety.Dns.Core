using System;

/*
 * http://tools.ietf.org/rfc/rfc2672.txt
 *
3. The DNAME Resource Record

   The DNAME RR has mnemonic DNAME and type code 39 (decimal).
   DNAME has the following format:

      <owner> <ttl> <class> DNAME <target>

   The format is not class-sensitive.  All fields are required.  The
   RDATA field <target> is a <domain-name> [DNSIS].

 *
 */
namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     DNAME DNS Record
    /// </summary>
    public class RecordDname : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordDname" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record</param>
        public RecordDname(RecordReader rr)
        {
            this.Target = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets or sets the target
        /// </summary>
        public String Target { get; set; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>String of the target</returns>
        public override String ToString()
        {
            return this.Target;
        }
    }
}
