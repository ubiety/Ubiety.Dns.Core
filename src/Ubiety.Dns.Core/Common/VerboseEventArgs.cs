using System;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Verbose event args
    /// </summary>
    public class VerboseEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VerboseEventArgs" /> class
        /// </summary>
        /// <param name="message">Verbose message</param>
        public VerboseEventArgs(String message)
        {
            this.Message = message;
        }

        /// <summary>
        ///     Gets or sets the message to output
        /// </summary>
        public String Message { get; set; }
    }
}