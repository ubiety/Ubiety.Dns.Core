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
using Xunit;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Covers the reverse lookup name helpers. Expected values are the canonical reverse DNS forms,
    /// derived independently of the implementation.
    /// </summary>
    public class ResolverArpaTests
    {
        [Theory]
        [InlineData("192.168.1.2", "2.1.168.192.in-addr.arpa.")]
        [InlineData("8.8.8.8", "8.8.8.8.in-addr.arpa.")]
        [InlineData("0.0.0.0", "0.0.0.0.in-addr.arpa.")]
        [InlineData("255.255.255.255", "255.255.255.255.in-addr.arpa.")]
        public void GetArpaFromIpReversesAnIPv4Address(string address, string expected)
        {
            Resolver.GetArpaFromIp(IPAddress.Parse(address)).ShouldBe(expected);
        }

        [Theory]
        [InlineData("::1", "1.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.ip6.arpa.")]
        [InlineData("2001:db8::1", "1.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.0.8.b.d.0.1.0.0.2.ip6.arpa.")]
        public void GetArpaFromIpReversesAnIPv6AddressNibbleByNibble(string address, string expected)
        {
            Resolver.GetArpaFromIp(IPAddress.Parse(address)).ShouldBe(expected);
        }

        [Fact]
        public void GetArpaFromIpRejectsNull()
        {
            Should.Throw<ArgumentNullException>(() => Resolver.GetArpaFromIp(null!));
        }

        [Theory]
        [InlineData("18005551212", "2.1.2.1.5.5.5.0.0.8.1.e164.arpa.")]
        [InlineData("+1 800 555 1212", "2.1.2.1.5.5.5.0.0.8.1.e164.arpa.")]
        [InlineData("+1-800-555-1212", "2.1.2.1.5.5.5.0.0.8.1.e164.arpa.")]
        public void GetArpaFromEnumeratorStripsNonDigitsAndReverses(string enumerator, string expected)
        {
            Resolver.GetArpaFromEnumerator(enumerator).ShouldBe(expected);
        }

        [Fact]
        public void GetArpaFromEnumeratorReturnsTheBareSuffixWhenThereAreNoDigits()
        {
            Resolver.GetArpaFromEnumerator("no digits here").ShouldBe("e164.arpa.");
        }
    }
}
