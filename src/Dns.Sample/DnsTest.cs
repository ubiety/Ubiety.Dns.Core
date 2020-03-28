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

using System.Collections.Generic;
using System.Linq;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;

namespace Dns.Sample
{
    public class DnsTest
    {
        private readonly Resolver _resolver;

        public DnsTest()
        {
            _resolver = ResolverBuilder.Begin()
                .SetTimeout(1000)
                .EnableCache()
                .SetRetries(3)
                .UseRecursion()
                .Build();
        }

        public IEnumerable<string> CertRecords(string name)
        {
            const QuestionType questionType = QuestionType.CERT;

            var response = _resolver.Query(name, questionType);

            return response.GetRecords<RecordCert>().Select(record => record.ToString()).ToList();
        }

        public IEnumerable<string> ARecords(string name)
        {
            const QuestionType questionType = QuestionType.A;

            var response = _resolver.Query(name, questionType);

            return response.GetRecords<RecordA>().Select(record => record.ToString()).ToList();
        }
    }
}