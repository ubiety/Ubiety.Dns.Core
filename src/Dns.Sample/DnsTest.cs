using System.Collections.Generic;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;

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

            foreach (var record in response.RecordCert)
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

            foreach (var record in response.RecordA)
            {
                records.Add(record.ToString());
            }

            return records;
        }
    }
}