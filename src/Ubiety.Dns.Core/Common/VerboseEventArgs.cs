/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
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