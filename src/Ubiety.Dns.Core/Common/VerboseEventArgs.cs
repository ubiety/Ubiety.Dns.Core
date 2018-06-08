using System;

namespace Ubiety.Dns.Core
{
    public partial class Resolver
    {
        /// <summary>
        ///     Verbose event args
        /// </summary>
        public class VerboseEventArgs : EventArgs
        {
            /// <summary>
            ///     Gets the message to output
            /// </summary>
            public string Message;

            /// <summary>
            ///     Initializes a new instance of the <see cref="VerboseEventArgs" /> class
            /// </summary>
            /// <param name="message">Verbose message</param>
            public VerboseEventArgs(string message)
            {
                this.Message = message;
            }
        }
    } 
}