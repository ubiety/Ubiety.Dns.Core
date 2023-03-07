/*
 * Copyright 2020 Dieter Lunn
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

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;

namespace Dns.Sample
{
    /// <summary>
    ///     Test DNS resolution
    /// </summary>
    public class DnsTest
    {
        private readonly Resolver _resolver;

        /// <summary>
        ///     Initialize a new instance of the <see cref="DnsTest"/> class
        /// </summary>
        /// <param name="dnsIp">IP of DNS server to use</param>
        public DnsTest(string dnsIp)
        {
            var factory = LoggerFactory.Create(b => b.AddConsole());
            var logger = factory.CreateLogger<Resolver>();


            _resolver = ResolverBuilder.Begin(logger)
                .AddDnsServer(dnsIp)
                .SetTimeout(1000)
                .EnableCache()
                .SetRetries(3)
                .UseRecursion()
                .Build();
        }

        /// <summary>
        ///     Test CERT record resolution
        /// </summary>
        /// <param name="name">URL of address to resolve</param>
        /// <returns>CERT records as a list of strings</returns>
        public IEnumerable<string> CertRecords(string name)
        {
            const QuestionType questionType = QuestionType.CERT;

            var response = _resolver.Query(name, questionType);

            return response.GetRecords<RecordCert>().Select(record => record.ToString()).ToList();
        }

        /// <summary>
        ///     Test A record resolution
        /// </summary>
        /// <param name="name">URL of address to resolve</param>
        /// <returns>A records as a list of strings</returns>
        public IEnumerable<string> ARecords(string name)
        {
            const QuestionType questionType = QuestionType.A;

            var response = _resolver.Query(name, questionType);

            return response.GetRecords<RecordA>().Select(record => record.ToString()).ToList();
        }

        /// <summary>
        ///     Test NAPTR record resolution
        /// </summary>
        /// <param name="domain">URL of address to resolve</param>
        /// <returns>NAPTR records as a list of strings</returns>
        public IEnumerable<RecordNaptr> GetNaptr(string domain)
        {
            var response = _resolver.Query(domain, QuestionType.NAPTR);

            return response.GetRecords<RecordNaptr>().ToList();
        }
    }
}
