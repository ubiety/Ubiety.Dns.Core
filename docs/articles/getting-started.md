# Getting started

## Install

Ubiety.Dns.Core targets .NET 10.

```shell
dotnet package add Ubiety.Dns.Core
```

## Your first query

A resolver is built with [ResolverBuilder](xref:Ubiety.Dns.Core.ResolverBuilder), then queried:

```csharp
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records.General;

var resolver = ResolverBuilder.Begin()
    .AddDnsServer("1.1.1.1")
    .SetTimeout(5000)
    .SetRetries(2)
    .UseRecursion()
    .EnableCache()
    .Build();

var response = resolver.Query("example.com", QuestionType.A);

foreach (var record in response.GetRecords<RecordA>())
{
    Console.WriteLine(record.Address);
}
```

If no server is added the builder falls back to the system resolvers, so
`ResolverBuilder.Begin().Build()` is a working resolver.

## Asynchronously

[QueryAsync](xref:Ubiety.Dns.Core.Resolver.QueryAsync*) is the asynchronous equivalent and takes a
cancellation token:

```csharp
var response = await resolver.QueryAsync("example.com", QuestionType.A, cancellationToken);
```

The async path is implemented independently down to the socket rather than wrapping the synchronous
one, so it does not block a thread.

Cancellation and timeout mean different things. A cancelled token abandons the query and throws
`OperationCanceledException`. An elapsed timeout is a transport failure: the resolver moves on to
the next server, and once every server and retry is exhausted it returns a response with
[TimedOut](xref:Ubiety.Dns.Core.Response.TimedOut) set.

## Reading the answer

A [Response](xref:Ubiety.Dns.Core.Response) carries the four sections of a DNS message.

```csharp
response.Header.ResponseCode   // NoError, NXDomain, ServFail, ...
response.Questions             // what was asked
response.Answers               // the answer section
response.Authorities           // the authority section
response.Additional            // the additional section
response.ResourceRecords       // answers, authorities and additional together
```

[GetRecords&lt;T&gt;](xref:Ubiety.Dns.Core.Response.GetRecords*) filters the **answer** section by
record type. To reach glue records in the additional section, read `Additional` directly.

```csharp
var addresses = response.GetRecords<RecordA>();          // answers only
var everything = response.ResourceRecords;               // all three sections
```

## Handling failure

A query that no server answers does not throw. It returns a response with `TimedOut` set and no
answers, so check that before reading records:

```csharp
var response = resolver.Query("example.com", QuestionType.A);

if (response.TimedOut)
{
    // no server replied
}
else if (response.Header.ResponseCode != ResponseCode.NoError)
{
    // the server replied with an error, for example NXDomain
}
```

`Query` throws only when the resolver is misconfigured, such as having no servers at all.
