# Copilot Instructions

## What this project is

Ubiety.Dns.Core is a DNS resolver library for .NET, published to NuGet. It builds DNS queries,
sends them over UDP or TCP, and parses the responses into typed record objects.

There is no web stack here: no ASP.NET Core, no Entity Framework, no HTTP, no controllers, no
dependency injection container. Suggestions built around those do not apply.

## Layout

| Path | Purpose |
| --- | --- |
| `src/Ubiety.Dns.Core` | The shipping library. The only packable project. |
| `src/Dns.Sample` | Small console program demonstrating the resolver. |
| `tests/Ubiety.Dns.Test` | xUnit tests using Shouldly for assertions. |
| `build/_build.csproj` | NUKE build. Not built by the solution; restore it separately. |
| `Directory.Build.props` | Shared MSBuild properties for every project. |
| `Directory.Packages.props` | Central Package Management. **All** package versions live here. |

Key types: `Resolver` and `ResolverBuilder` drive queries, `Request`/`Response` model a DNS
exchange, `RecordReader` parses the wire format, and `Records/*` hold one class per record type
dispatched via `RecordAttribute`.

## Build and test

```shell
dotnet build Ubiety.Dns.Core.sln
dotnet test tests/Ubiety.Dns.Test/Ubiety.Dns.Test.csproj
./build.sh Test              # the NUKE path CI uses; build.cmd on Windows
```

The build must stay at **zero warnings**. `TreatWarningsAsErrors` is on, so a new warning fails the
build. NuGet audit warnings (NU1901-NU1904) are the deliberate exception and remain warnings.

## Conventions

- **Never put a version on a `PackageReference`.** Add a `PackageVersion` to
  `Directory.Packages.props` instead. `PackageDownload` items are exempt.
- **Nullable reference types are enabled everywhere**, with nullable warnings as errors. Annotate
  honestly: if something can return null, its type is `T?`. Do not reach for `!` or `null!` to
  silence a warning — each existing use carries a comment justifying it.
- **StyleCop runs on the library** and public members need XML documentation. SA1010 is disabled in
  `.editorconfig` because it predates collection expressions.
- Target-typed `new`, collection expressions, file-scoped namespaces and primary constructors are
  all used and welcome.
- Keep the Apache-2.0 header at the top of new source files.

## Parsing untrusted input

`RecordReader` and everything under `Records/` parse bytes that arrive from the network and cannot
be trusted. When touching that code:

- Bound every loop. Domain name decompression follows pointers and is capped at 128 jumps and 255
  octets specifically to stop a malicious response from looping or exhausting the stack.
- Prefer tolerating malformed input by stopping early over throwing, matching the surrounding code.
  Serialization of caller-supplied data is the opposite: `Question.WriteName` throws
  `FormatException` on a name it cannot encode.
- Read exactly what the length prefix promises. Use `Stream.ReadExactly`, never a bare
  `Stream.Read` whose return value is discarded.

## Testing

- xUnit with Shouldly. `Should.Throw<T>` for exceptions, `ShouldBe` for equality.
- Prefer building a byte array that mirrors real wire data and asserting on the parsed result over
  mocking. Several tests embed a real DNS reply, compression pointers included.
- New parsing code needs a malformed-input test, not only a happy-path one.
