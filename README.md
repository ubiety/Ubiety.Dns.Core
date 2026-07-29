# ![Logo](https://github.com/ubiety/Ubiety.Dns.Core/raw/develop/library64.png) Ubiety.Dns.Core [![Nuget](https://img.shields.io/nuget/v/Ubiety.Dns.Core.svg?style=flat-square)](https://www.nuget.org/packages/Ubiety.Dns.Core/)

> A reusable DNS resolver for .NET

Thank you to the initial work of Alphons van der Heijden and Geoffry Huntley on this library.

| Branch  | Quality                                                                                                                                                                                                                                                                                                 | Appveyor                                                                                                                                                                                   | Coverage                                                                                                                                                        |
| ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Main    | [![Codacy branch grade](https://img.shields.io/codacy/grade/8f394c2975b44792b37aaf9b4f4bc3ec/main?style=flat-square)](https://www.codacy.com/gh/ubiety/Ubiety.Dns.Core/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ubiety/Ubiety.Dns.Core&amp;utm_campaign=Badge_Grade)     | [![AppVeyor branch](https://img.shields.io/appveyor/ci/coder2000/ubiety-dns-core/main.svg?style=flat-square)](https://ci.appveyor.com/project/coder2000/ubiety-dns-core/branch/main)       | [![Codecov branch](https://img.shields.io/codecov/c/github/ubiety/Ubiety.Dns.Core/main.svg?style=flat-square)](https://codecov.io/gh/ubiety/Ubiety.Dns.Core)    |
| Develop | [![Codacy branch grade](https://img.shields.io/codacy/grade/8f394c2975b44792b37aaf9b4f4bc3ec/develop?style=flat-square)](https://www.codacy.com/gh/ubiety/Ubiety.Dns.Core/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ubiety/Ubiety.Dns.Core&amp;utm_campaign=Badge_Grade)  | [![AppVeyor branch](https://img.shields.io/appveyor/ci/coder2000/ubiety-dns-core/develop.svg?style=flat-square)](https://ci.appveyor.com/project/coder2000/ubiety-dns-core/branch/develop) | [![Codecov branch](https://img.shields.io/codecov/c/github/ubiety/Ubiety.Dns.Core/develop.svg?style=flat-square)](https://codecov.io/gh/ubiety/Ubiety.Dns.Core) |

## Installing / Getting started

Ubiety DNS Core targets .NET 10 and is available from NuGet

```shell
dotnet package add Ubiety.Dns.Core
```

You can also use your favorite NuGet client.

## Usage

Build a resolver with `ResolverBuilder`, then query it and pull out the record
type you care about:

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

There is an async equivalent that takes a cancellation token:

```csharp
var response = await resolver.QueryAsync("example.com", QuestionType.A, cancellationToken);
```

The question class defaults to `IN`; pass it explicitly when you need another one:

```csharp
var response = await resolver.QueryAsync(
    "example.com", QuestionType.A, QuestionClass.CH, cancellationToken);
```

`QueryAsync` is asynchronous the whole way down rather than the synchronous path wrapped in a task.
A cancelled token abandons the query and throws `OperationCanceledException`; that is distinct from
the configured timeout, which fails over to the next server and eventually returns a `Response` with
`TimedOut` set. A cached answer is returned without awaiting anything.

If no DNS server is added the builder falls back to the system resolvers.
Queries go over TCP by default; set `resolver.TransportType = TransportType.Udp`
to use UDP instead. A query that no server answers returns a `Response` with
`TimedOut` set rather than throwing.

`src/Dns.Sample` is a small runnable program that does the same thing from the
command line.

## Developing

Here's a brief intro about what a developer must do in order to start developing
the project further:

```shell
git clone https://github.com/ubiety/Ubiety.Dns.Core.git
cd Ubiety.Dns.Core
dotnet restore
```

Clone the repository and then restore the development requirements. You can use
any editor: Rider, VS Code or Visual Studio. Building requires the .NET SDK
version pinned in `global.json`.

### Building

The build is driven by [NUKE](https://github.com/nuke-build/nuke). Use
`build.cmd` on Windows and `build.sh` elsewhere:

```shell
./build.sh Compile     # restore and compile
./build.sh Test        # run the tests with coverage
./build.sh Pack        # produce the NuGet package in ./artifacts
./build.sh Docs        # build the documentation site into docs/_site
```

Running `dotnet build` and `dotnet test` directly works too.

### Documentation

The site is [DocFX](https://dotnet.github.io/docfx/). The API reference is generated from the XML
documentation comments in the source, so it cannot drift from the code; the guides under
`docs/articles` are hand written.

```shell
dotnet tool restore
dotnet docfx docs/docfx.json --serve    # http://localhost:8080
```

The site deploys to Netlify from `netlify.toml`. The Netlify build image has no .NET, so
`docs/netlify-build.sh` installs the SDK version pinned in `global.json` before running DocFX,
caching it under `/opt/build/cache` so later builds skip the download.

## Contributing

Thank you for your assistance. Please find more information on how to contribute in the CONTRIBUTING.md

## Links

- Project homepage: <https://dns.ubiety.dev>
- Repository: <https://github.com/ubiety/Ubiety.Dns.Core/>
- Issue tracker: <https://github.com/ubiety/Ubiety.Dns.Core/issues>
  - In case of sensitive bugs like security vulnerabilities, please use the
    [Tidelift security contact](https://tidelift.com/security) instead of using issue tracker.
    We value your effort to improve the security and privacy of this project! Tidelift will coordinate the fix and disclosure.
- Related projects:
  - Ubiety VersionIt: <https://github.com/ubiety/Ubiety.VersionIt/>
  - Ubiety Toolset: <https://github.com/ubiety/Ubiety.Toolset/>
  - Ubiety Xmpp: <https://github.com/ubiety/Ubiety.Xmpp.Core/>
  - Ubiety Stringprep: <https://github.com/ubiety/Ubiety.Stringprep.Core/>
  - Ubiety SCRAM: <https://github.com/ubiety/Ubiety.Scram.Core/>

## Ubiety.Dns.Core for enterprise

Available as part of the Tidelift Subscription

The maintainers of Ubiety.Dns.Core and thousands of other packages are working with Tidelift to deliver commercial support and maintenance for the open source dependencies you use to build your applications. Save time, reduce risk, and improve code health, while paying the maintainers of the exact dependencies you use. [Learn more.](https://tidelift.com/subscription/pkg/nuget-ubiety-dns-core?utm_source=nuget-ubiety-dns-core&utm_medium=referral&utm_campaign=enterprise&utm_term=repo)

## Sponsors

### Gold Sponsors

[![Gold Sponsors](https://opencollective.com/ubiety/tiers/gold-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

### Silver Sponsors

[![Silver Sponsors](https://opencollective.com/ubiety/tiers/silver-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)

### Bronze Sponsors

[![Bronze Sponsors](https://opencollective.com/ubiety/tiers/bronze-sponsor.svg?avatarHeight=36)](https://opencollective.com/ubiety/)
