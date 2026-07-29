---
_layout: landing
---

# Ubiety.Dns.Core

A reusable DNS resolver for .NET. It builds DNS queries, sends them over UDP or TCP, and parses the
replies into typed record objects.

```csharp
var resolver = ResolverBuilder.Begin()
    .AddDnsServer("1.1.1.1")
    .UseRecursion()
    .EnableCache()
    .Build();

var response = await resolver.QueryAsync("example.com", QuestionType.A);

foreach (var record in response.GetRecords<RecordA>())
{
    Console.WriteLine(record.Address);
}
```

- [Getting started](articles/getting-started.md) — install, query, read the answers
- [Resolver configuration](articles/configuration.md) — servers, transport, timeouts, retries, caching
- [Record types](articles/record-types.md) — what the library parses and how to reach it
- [API reference](api/index.md) — generated from the source

Thank you to the initial work of Alphons van der Heijden and Geoffry Huntley on this library.
