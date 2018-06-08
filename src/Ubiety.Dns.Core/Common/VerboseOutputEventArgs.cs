using System;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     Event args for verbose output
    /// </summary>
    public class VerboseOutputEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VerboseOutputEventArgs" /> class
        /// </summary>
        /// <param name="message">Message to output</param>
        public VerboseOutputEventArgs(String message)
        {
            this.Message = message;
        }

        /// <summary>
        ///     Gets or sets the string message
        /// </summary>
        public String Message { get; set; }
    }
}