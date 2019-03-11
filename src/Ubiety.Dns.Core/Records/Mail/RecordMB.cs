/*
3.3.3. MB RDATA format (EXPERIMENTAL)

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   MADNAME                     /
    /                                               /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

MADNAME         A <domain-name> which specifies a host which has the
                specified mailbox.

MB records cause additional section processing which looks up an A type
RRs corresponding to MADNAME.
*/

namespace Ubiety.Dns.Core.Records.Mail
{
    /// <summary>
    ///     Mailbox DNS record.
    /// </summary>
    public class RecordMb : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordMb" /> class.
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data.</param>
        public RecordMb(RecordReader rr)
        {
            MadName = rr.ReadDomainName();
        }

        /// <summary>
        ///     Gets the mailbox domain.
        /// </summary>
        public string MadName { get; }

        /// <summary>
        ///     String representation of the record data.
        /// </summary>
        /// <returns>String version of the domain.</returns>
        public override string ToString()
        {
            return MadName;
        }
    }
}