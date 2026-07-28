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

using System;
using System.CommandLine;

namespace Dns.Sample
{
    internal static class Program
    {
        /// <summary>
        ///     Main application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The process exit code</returns>
        public static int Main(string[] args)
        {
            var dnsIpOption = new Option<string>("--dns-ip", "-d")
            {
                Description = "IP address of DNS server",
                Required = true,
            };

            var rootCommand = new RootCommand("Sample queries against the Ubiety DNS resolver");
            rootCommand.Options.Add(dnsIpOption);

            rootCommand.SetAction(parseResult => Query(parseResult.GetRequiredValue(dnsIpOption)));

            return rootCommand.Parse(args).Invoke();
        }

        private static void Query(string dnsIp)
        {
            var test = new DnsTest(dnsIp);

            foreach (var record in test.CertRecords("direct.sitenv.org"))
            {
                Console.WriteLine(record);
            }

            Console.WriteLine();

            foreach (var record in test.ARecords("direct.sitenv.org"))
            {
                Console.WriteLine(record);
            }

            Console.WriteLine();

            foreach (var record in test.GetNaptr("dev.nwise.se"))
            {
                Console.WriteLine(record);
            }
        }
    }
}
