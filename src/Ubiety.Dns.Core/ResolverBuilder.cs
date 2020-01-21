/*
 * Licensed under the MIT license
 * See the LICENSE file in the project root for more information
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
