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
    ///     Event args for verbose output.
    /// </summary>
    public class VerboseOutputEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VerboseOutputEventArgs" /> class.
        /// </summary>
        /// <param name="message">Message to output.</param>
        public VerboseOutputEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        ///     Gets the string message.
        /// </summary>
        public string Message { get; }
    }
}