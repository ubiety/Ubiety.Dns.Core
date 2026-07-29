/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

using Ubiety.Logging.Core;

namespace Ubiety.Dns.Core;

/// <summary>
/// Provides a builder for configuring and creating a DNS resolver.
/// </summary>
public class ResolverBuilder
{
    private readonly List<IPEndPoint> _dnsServers;
    private IUbietyLogManager? _logManager;
    private int _timeout;
    private bool _enableCache;
    private int _retries;
    private bool _useRecursion;

    private ResolverBuilder()
    {
        _dnsServers = [];
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ResolverBuilder"/> class.
    /// </summary>
    /// <returns>A new <see cref="ResolverBuilder"/> instance for configuring a DNS resolver.</returns>
    public static ResolverBuilder Begin()
    {
        return new();
    }

    /// <summary>
    /// Enables logging for the DNS resolver.
    /// </summary>
    /// <param name="logManager">An <see cref="IUbietyLogManager"/> instance to be used for logging.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="logManager"/> is null.</exception>
    public ResolverBuilder EnableLogging(IUbietyLogManager logManager)
    {
        ArgumentNullException.ThrowIfNull(logManager);

        _logManager = logManager;
        return this;
    }

    /// <summary>
    /// Adds a DNS server to the resolver.
    /// </summary>
    /// <param name="server">The <see cref="IPEndPoint"/> representing the DNS server.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="server"/> is null.</exception>
    public ResolverBuilder AddDnsServer(IPEndPoint server)
    {
        ArgumentNullException.ThrowIfNull(server);

        _dnsServers.Add(server);

        return this;
    }

    /// <summary>
    /// Adds a DNS server to the resolver.
    /// </summary>
    /// <param name="serverAddress">The <see cref="IPAddress"/> of the DNS server.</param>
    /// <param name="port">The port number of the DNS server.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="serverAddress"/> is null.</exception>
    public ResolverBuilder AddDnsServer(IPAddress serverAddress, int port)
    {
        // IPEndPoint would throw for null anyway, but naming its own parameter rather than ours.
        ArgumentNullException.ThrowIfNull(serverAddress);

        _dnsServers.Add(new IPEndPoint(serverAddress, port));

        return this;
    }

    /// <summary>
    /// Adds a DNS server to the resolver.
    /// </summary>
    /// <param name="serverAddress">The <see cref="IPAddress"/> representing the DNS server to add.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    public ResolverBuilder AddDnsServer(IPAddress serverAddress)
    {
        return AddDnsServer(serverAddress, 53);
    }

    /// <summary>
    /// Adds a DNS server to the resolver.
    /// </summary>
    /// <param name="serverAddress">The string representing the DNS server to be added.</param>
    /// <param name="port">The port number of the DNS server.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="serverAddress"/> is null.</exception>
    /// <remarks>
    /// A null address is a caller mistake and throws. An address that is merely unparseable is
    /// dropped and the builder returned unchanged, since such a value may come from configuration.
    /// </remarks>
    public ResolverBuilder AddDnsServer(string serverAddress, int port)
    {
        ArgumentNullException.ThrowIfNull(serverAddress);

        return IPAddress.TryParse(serverAddress, out var serverIp) ? AddDnsServer(serverIp, port) : this;
    }

    /// <summary>
    /// Adds a DNS server to the resolver.
    /// </summary>
    /// <param name="serverAddress">The endpoint of the DNS server to be added.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    public ResolverBuilder AddDnsServer(string serverAddress)
    {
        return AddDnsServer(serverAddress, 53);
    }

    /// <summary>
    /// Adds multiple DNS servers to the resolver.
    /// </summary>
    /// <param name="dnsServers">A collection of <see cref="IPEndPoint"/> representing the DNS servers to be added.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance for further configuration.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dnsServers"/> is null or contains a null entry.
    /// </exception>
    public ResolverBuilder AddDnsServers(IEnumerable<IPEndPoint> dnsServers)
    {
        ArgumentNullException.ThrowIfNull(dnsServers);

        // Materialise once, both to avoid enumerating twice and so a null entry is rejected before
        // any of the collection is applied.
        var servers = dnsServers.ToList();
        if (servers.Exists(server => server is null))
        {
            throw new ArgumentNullException(nameof(dnsServers), "The collection contains a null server.");
        }

        _dnsServers.AddRange(servers);

        return this;
    }

    /// <summary>
    /// Sets the timeout duration for DNS resolver requests.
    /// </summary>
    /// <param name="timeout">The time in milliseconds to wait for a response before timing out.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    public ResolverBuilder SetTimeout(int timeout)
    {
        _timeout = timeout;

        return this;
    }

    /// <summary>
    /// Enables caching for the DNS resolver.
    /// </summary>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    public ResolverBuilder EnableCache()
    {
        _enableCache = true;

        return this;
    }

    /// <summary>
    /// Sets the number of retry attempts for DNS resolution.
    /// </summary>
    /// <param name="retries">The number of retries to attempt.</param>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    public ResolverBuilder SetRetries(int retries)
    {
        _retries = retries;

        return this;
    }

    /// <summary>
    /// Enables recursion for the DNS resolver.
    /// </summary>
    /// <returns>The current <see cref="ResolverBuilder"/> instance.</returns>
    public ResolverBuilder UseRecursion()
    {
        _useRecursion = true;

        return this;
    }

    /// <summary>
    /// Builds and returns a configured instance of the <see cref="Resolver"/> class.
    /// </summary>
    /// <returns>A configured <see cref="Resolver"/> instance.</returns>
    public Resolver Build()
    {
        if (_logManager != null)
        {
            UbietyLogger.Initialize(_logManager);
        }

        if (_dnsServers.Count < 1)
        {
            AddSystemServers();
        }

        if (_timeout == 0)
        {
            _timeout = 1000;
        }

        if (_retries == 0)
        {
            _retries = 1;
        }

        return new Resolver(_dnsServers) { Timeout = _timeout, UseCache = _enableCache, Retries = _retries, Recursion = _useRecursion };
    }

    private void AddSystemServers()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var adapter in interfaces)
        {
            if (adapter.OperationalStatus != OperationalStatus.Up)
            {
                continue;
            }

            var interfaceProperties = adapter.GetIPProperties();

            // thanks to Jon Webster on May 20, 2008
            foreach (var address in interfaceProperties.DnsAddresses)
            {
                var entry = new IPEndPoint(address, 53);
                if (!_dnsServers.Contains(entry))
                {
                    _dnsServers.Add(entry);
                }
            }
        }
    }
}
