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

using System.IO;
using System.Net;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records.General;
using Xunit;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Covers the response cache without touching the network. ReceiveResponse populates the cache
    /// as a side effect, so seeding it that way makes a subsequent Query resolve from memory.
    /// </summary>
    public class ResolverCacheTests
    {
        private static readonly IPEndPoint Server = new(IPAddress.Loopback, 53);

        private static byte[] AnswerFor(string name, uint ttl, params byte[] address) =>
            DnsMessageBuilder.Framed(DnsMessageBuilder.Message(
                0x4242,
                DnsMessageBuilder.NoErrorFlags,
                name,
                QuestionType.A,
                DnsMessageBuilder.ARecord(name, ttl, address)));

        // A closed high port on loopback refuses instantly, so a cache miss fails fast and
        // deterministically instead of depending on whether local DNS software occupies port 53.
        private const int ClosedPort = 59353;

        private static Resolver CachingResolver() =>
            ResolverBuilder.Begin().AddDnsServer(IPAddress.Loopback, ClosedPort).EnableCache().Build();

        [Fact]
        public void QueryReturnsACachedResponseWithoutContactingAServer()
        {
            var resolver = CachingResolver();

            // Seed the cache. The resolver's only server is a closed port, so a cache miss here
            // would fail rather than quietly resolving.
            var seeded = resolver.ReceiveResponse(
                new MemoryStream(AnswerFor("example.com", 300, 5, 6, 7, 8)), Server);

            var cached = resolver.Query("example.com", QuestionType.A);

            cached.ShouldBeSameAs(seeded);
            cached.GetRecords<RecordA>().ShouldHaveSingleItem()
                .Address.ShouldBe(IPAddress.Parse("5.6.7.8"));
        }

        [Fact]
        public void ClearCacheDiscardsASeededResponse()
        {
            var resolver = CachingResolver();
            resolver.ReceiveResponse(
                new MemoryStream(AnswerFor("example.com", 300, 5, 6, 7, 8)), Server);

            resolver.ClearCache();

            // With the cache emptied the query falls through to the network and fails fast against
            // the closed port, rather than returning the seeded answer.
            resolver.Query("example.com", QuestionType.A).TimedOut.ShouldBeTrue();
        }

        [Fact]
        public void ADifferentQuestionIsNotServedFromTheCache()
        {
            var resolver = CachingResolver();
            resolver.ReceiveResponse(
                new MemoryStream(AnswerFor("example.com", 300, 5, 6, 7, 8)), Server);

            // Same name, different type, so the cache key does not match.
            resolver.Query("example.com", QuestionType.MX).TimedOut.ShouldBeTrue();
        }

        [Fact]
        public void NothingIsCachedWhenCachingIsDisabled()
        {
            var resolver = ResolverBuilder.Begin().AddDnsServer(IPAddress.Loopback, ClosedPort).Build();
            resolver.ReceiveResponse(
                new MemoryStream(AnswerFor("example.com", 300, 5, 6, 7, 8)), Server);

            resolver.Query("example.com", QuestionType.A).TimedOut.ShouldBeTrue();
        }

        [Fact]
        public void AnExpiredRecordIsNotServedFromTheCache()
        {
            var resolver = CachingResolver();

            // A zero TTL is expired the moment it is stored.
            resolver.ReceiveResponse(
                new MemoryStream(AnswerFor("example.com", 0, 5, 6, 7, 8)), Server);

            resolver.Query("example.com", QuestionType.A).TimedOut.ShouldBeTrue();
        }
    }
}
