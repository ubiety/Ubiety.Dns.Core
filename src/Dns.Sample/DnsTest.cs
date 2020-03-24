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
            _resolver = new Resolver()
            {
                Recursion = true,
                UseCache = true,
                Timeout = 1000,
                Retries = 3,
                TransportType = TransportType.Tcp
            };
        }

        public IList<string> CertRecords(string name)
        {
            IList<string> records = new List<string>();

            const QuestionType questionType = QuestionType.CERT;
            const QuestionClass questionClass = QuestionClass.IN;

            var response = _resolver.Query(name, questionType, questionClass);

            foreach (var record in response.GetRecords<RecordCert>())
            {
                records.Add(record.ToString());
            }

            return records;
        }

        public IList<string> ARecords(string name)
        {
            IList<string> records = new List<string>();

            const QuestionType questionType = QuestionType.A;
            const QuestionClass questionClass = QuestionClass.IN;

            var response = _resolver.Query(name, questionType, questionClass);

            foreach (var record in response.GetRecords<RecordA>())
            {
                records.Add(record.ToString());
            }

            return records;
        }
    }
}