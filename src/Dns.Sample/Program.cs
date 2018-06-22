using System;

namespace Dns.Sample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var test = new DnsTest();
            
            Console.WriteLine("CERT records for: direct.sitenv.org");
            foreach (var record in test.CertRecords("direct.sitenv.org"))
            {
                Console.WriteLine(record);
            }
        }
    }
}