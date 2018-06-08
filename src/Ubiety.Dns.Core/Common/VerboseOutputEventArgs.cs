using System;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     Event args for verbose output
    /// </summary>
    public class VerboseOutputEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the string message
        /// </summary>
        public string Message;

        /// <summary>
        ///     Initializes a new instance of the <see cref="VerboseOutputEventArgs" /> class
        /// </summary>
        /// <param name="message">Message to output</param>
        public VerboseOutputEventArgs(string message)
        {
            this.Message = message;
        }
    }
}