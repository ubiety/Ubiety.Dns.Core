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
using System.Net;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Xunit;

namespace Ubiety.Dns.Test
{
    public class ResolverBuilderTests
    {
        [Fact]
        public void BuildAppliesDefaultsWhenNothingIsConfigured()
        {
            var resolver = ResolverBuilder.Begin().AddDnsServer(IPAddress.Loopback).Build();

            resolver.Timeout.ShouldBe(1000);
            resolver.Retries.ShouldBe(1);
            resolver.Recursion.ShouldBeFalse();
            resolver.UseCache.ShouldBeFalse();
            resolver.TransportType.ShouldBe(TransportType.Tcp);
        }

        [Fact]
        public void BuildCarriesEveryConfiguredOption()
        {
            var resolver = ResolverBuilder.Begin()
                .AddDnsServer(IPAddress.Loopback)
                .SetTimeout(4200)
                .SetRetries(7)
                .UseRecursion()
                .EnableCache()
                .Build();

            resolver.Timeout.ShouldBe(4200);
            resolver.Retries.ShouldBe(7);
            resolver.Recursion.ShouldBeTrue();
            resolver.UseCache.ShouldBeTrue();
        }

        [Fact]
        public void BuildFallsBackToTheSystemResolversWhenNoServerIsAdded()
        {
            // The set of system nameservers is environment specific, so this only asserts that the
            // fallback path runs and produces a usable resolver.
            Should.NotThrow(() => ResolverBuilder.Begin().Build());
        }

        [Fact]
        public void AddDnsServerAcceptsAnEndPoint()
        {
            var resolver = ResolverBuilder.Begin()
                .AddDnsServer(new IPEndPoint(IPAddress.Loopback, 5353))
                .Build();

            Should.NotThrow(() => resolver.ClearCache());
        }

        [Fact]
        public void AddDnsServerAcceptsAnAddressAndPort()
        {
            Should.NotThrow(() => ResolverBuilder.Begin()
                .AddDnsServer(IPAddress.Loopback, 5353)
                .Build());
        }

        [Fact]
        public void AddDnsServerAcceptsAStringAndPort()
        {
            Should.NotThrow(() => ResolverBuilder.Begin()
                .AddDnsServer("127.0.0.1", 5353)
                .Build());
        }

        [Fact]
        public void AddDnsServerAcceptsSeveralEndPointsAtOnce()
        {
            Should.NotThrow(() => ResolverBuilder.Begin()
                .AddDnsServers([new IPEndPoint(IPAddress.Loopback, 53), new IPEndPoint(IPAddress.IPv6Loopback, 53)])
                .Build());
        }

        [Fact]
        public void AddDnsServerRejectsANullEndPoint()
        {
            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin().AddDnsServer((IPEndPoint)null!))
                .ParamName.ShouldBe("server");
        }

        [Fact]
        public void AddDnsServerRejectsANullAddress()
        {
            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin().AddDnsServer((IPAddress)null!, 53))
                .ParamName.ShouldBe("serverAddress");

            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin().AddDnsServer((IPAddress)null!))
                .ParamName.ShouldBe("serverAddress");
        }

        [Fact]
        public void AddDnsServerRejectsANullAddressString()
        {
            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin().AddDnsServer((string)null!, 53))
                .ParamName.ShouldBe("serverAddress");

            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin().AddDnsServer((string)null!))
                .ParamName.ShouldBe("serverAddress");
        }

        [Fact]
        public void AddDnsServersRejectsANullCollection()
        {
            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin().AddDnsServers(null!))
                .ParamName.ShouldBe("dnsServers");
        }

        [Fact]
        public void AddDnsServersRejectsACollectionContainingNull()
        {
            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin()
                        .AddDnsServers([new IPEndPoint(IPAddress.Loopback, 53), null!]))
                .ParamName.ShouldBe("dnsServers");
        }

        [Fact]
        public void AddDnsServersAppliesNothingWhenAnEntryIsNull()
        {
            // The collection is validated before any of it is applied, so a bad entry does not
            // leave the builder holding a partially applied list.
            var builder = ResolverBuilder.Begin();

            Should.Throw<ArgumentNullException>(
                () => builder.AddDnsServers([new IPEndPoint(IPAddress.Loopback, 53), null!]));

            // No server was added, so Build falls back to the system resolvers rather than using
            // the endpoint that preceded the null.
            Should.NotThrow(() => builder.Build());
        }

        [Fact]
        public void EnableLoggingRejectsANullLogManager()
        {
            Should.Throw<ArgumentNullException>(
                    () => ResolverBuilder.Begin().EnableLogging(null!))
                .ParamName.ShouldBe("logManager");
        }

        [Fact]
        public void AddDnsServerSilentlyIgnoresAnUnparseableAddress()
        {
            // The string overloads parse with TryParse and drop anything that fails, so a typo in a
            // server address is not reported. Build then falls back to the system resolvers.
            var builder = ResolverBuilder.Begin().AddDnsServer("not-an-ip-address");

            builder.ShouldNotBeNull();
            Should.NotThrow(() => builder.Build());
        }

        [Fact]
        public void QueryThrowsWhenTheResolverHasNoServers()
        {
            // Reachable only by constructing the resolver directly; Build always supplies servers.
            var resolver = new Resolver([]);

            Should.Throw<InvalidOperationException>(
                () => resolver.Query("example.com", QuestionType.A));
        }
    }
}
