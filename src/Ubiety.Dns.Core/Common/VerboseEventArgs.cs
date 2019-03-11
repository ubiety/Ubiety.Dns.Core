/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
 */

using System;

namespace Ubiety.Dns.Core.Common
{
    /// <summary>
    ///     Verbose event args.
    /// </summary>
    public class VerboseEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VerboseEventArgs" /> class.
        /// </summary>
        /// <param name="message">Verbose message.</param>
        public VerboseEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        ///     Gets or sets the message to output.
        /// </summary>
        public string Message { get; set; }
    }
}