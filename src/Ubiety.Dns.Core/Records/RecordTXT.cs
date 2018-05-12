using System;

#region Rfc info
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
#endregion

namespace Heijden.DNS
{
        /// <summary>
        /// </summary>
    public class RecordTXT : Record
    {
        /// <summary>
        /// </summary>
        public string TXT;

        /// <summary>
        /// </summary>
        public RecordTXT(RecordReader rr)
        {
            TXT = rr.ReadString();
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return string.Format("\"{0}\"",TXT);
        }

    }
}
