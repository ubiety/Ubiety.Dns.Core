/*
 3.3.2. HINFO RDATA format

    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                      CPU                      /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
    /                       OS                      /
    +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

where:

CPU             A <character-string> which specifies the CPU type.

OS              A <character-string> which specifies the operating
                system type.

Standard values for CPU and OS can be found in [RFC-1010].

HINFO records are used to acquire general information about a host.  The
main use is for protocols such as FTP that can use special procedures
when talking between machines or operating systems of the same type.
 */

using System;

namespace Ubiety.Dns.Core.Records
{
    /// <summary>
    ///     Hardware info DNS record
    /// </summary>
    public class RecordHinfo : Record
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordHinfo" /> class
        /// </summary>
        /// <param name="rr"><see cref="RecordReader" /> for the record data</param>
        public RecordHinfo(RecordReader rr)
        {
            Cpu = rr.ReadString();
            Os = rr.ReadString();
        }

        /// <summary>
        ///     Gets the CPU
        /// </summary>
        public string Cpu { get; }

        /// <summary>
        ///     Gets the OS
        /// </summary>
        public string Os { get; }

        /// <summary>
        ///     String representation of the record data
        /// </summary>
        /// <returns>String version of the record</returns>
        public override string ToString()
        {
            return $"CPU={Cpu} OS={Os}";
        }
    }
}