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

using Ubiety.Logging.Core;

namespace Ubiety.Dns.Core
{
    /// <summary>
    ///     Builds a resolver instance.
    /// </summary>
    public class ResolverBuilder
    {
        private IUbietyLogManager _logManager;

        /// <summary>
        ///     Begin building a DNS resolver.
        /// </summary>
        /// <returns>A <see cref="ResolverBuilder"/> instance.</returns>
        public static ResolverBuilder Begin()
        {
            return new ResolverBuilder();
        }

        /// <summary>
        ///     Enable logging for the resolver.
        /// </summary>
        /// <param name="logManager"><see cref="IUbietyLogManager"/> instance to use for logging.</param>
        /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
        public ResolverBuilder EnableLogging(IUbietyLogManager logManager)
        {
            _logManager = logManager;
            return this;
        }

        /// <summary>
        ///     Build the resolver instance with options provided.
        /// </summary>
        /// <returns>A <see cref="Resolver"/> instance.</returns>
        public Resolver Build()
        {
            if (_logManager != null)
            {
                UbietyLogger.Initialize(_logManager);
            }

            return new Resolver();
        }
    }
}
