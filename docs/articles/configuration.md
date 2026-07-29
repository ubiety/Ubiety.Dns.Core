# Resolver configuration

Everything is set through [ResolverBuilder](xref:Ubiety.Dns.Core.ResolverBuilder), which returns
itself from each call so it chains.

```csharp
var resolver = ResolverBuilder.Begin()
    .AddDnsServer("1.1.1.1")
    .AddDnsServer(IPAddress.Parse("8.8.8.8"))
    .SetTimeout(5000)
    .SetRetries(2)
    .UseRecursion()
    .EnableCache()
    .EnableLogging(logManager)
    .Build();
```

## Servers

| Overload | Notes |
| --- | --- |
| `AddDnsServer(IPEndPoint)` | Full control over address and port. |
| `AddDnsServer(IPAddress)` | Port 53. |
| `AddDnsServer(IPAddress, int)` | Explicit port. |
| `AddDnsServer(string)` | Parsed; port 53. |
| `AddDnsServer(string, int)` | Parsed; explicit port. |
| `AddDnsServers(IEnumerable<IPEndPoint>)` | Several at once. |

Servers are tried in the order added. Passing null throws `ArgumentNullException`. A string that is
not a valid address is a different case: it is dropped and the builder returned unchanged, on the
grounds that such a value may have come from configuration rather than from a mistake in code.

If no server is added, `Build` falls back to the addresses configured on the machine's network
interfaces.

## Transport

Queries go over **TCP by default**. Switch after building:

```csharp
resolver.TransportType = TransportType.Udp;
```

TCP is the more capable choice: UDP replies are size limited, and a zone transfer only works over
TCP. UDP avoids a connection handshake and is usually quicker for ordinary lookups.

## Timeout and retries

`SetTimeout` is milliseconds and applies to both transports; it defaults to 1000. `SetRetries`
defaults to 1 and counts full passes over the server list, so two servers with three retries is up
to six attempts before the resolver gives up and returns a timed out response.

A timeout is not an error you catch. It fails over to the next server, and only if every attempt
fails do you get a response with `TimedOut` set.

## Caching

`EnableCache` keeps successful, error-free responses keyed by question. A cached answer is returned
without touching the network, including from `QueryAsync`, which returns without awaiting anything.

Entries are dropped once any record in the response outlives its TTL. `ClearCache` empties it.

Caching is off by default.

## Logging

`EnableLogging` takes an `IUbietyLogManager` from
[Ubiety.Logging.Core](https://github.com/ubiety/Ubiety.Logging.Core). Without one the library logs
nothing.
