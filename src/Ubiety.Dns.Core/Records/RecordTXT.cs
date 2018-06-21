

/*
3.3.14. TXT RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                   TXT-DATA                    /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

TXT-DATA        One or more <character-string>s.

TXT RRs are used to hold descriptive text.  The semantics of the text
depends on the domain where it is found.
 *
*/

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Text DNS record
    /// </summary>
    public class RecordTxt : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordTxt" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordTxt(RecordReader rr)
        {
            Text = rr.ReadString();
        }

        /// <summary>
        ///     Gets or sets the text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>Text as a string</returns>
        public override string ToString()
        {
            return $"\"{Text}\"";
        }
    }
}