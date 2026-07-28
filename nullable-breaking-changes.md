# Nullable reference type annotations — consumer impact

`Ubiety.Dns.Core` now builds with `<Nullable>enable</Nullable>`. Nullable annotations are part of
the public API contract, so this page records what changed for consumers.

None of these are **binary** breaking changes — no value type gained `Nullable<T>`, and no signature
changed in a way that affects the emitted metadata beyond nullability attributes. Existing compiled
assemblies keep working without recompilation.

They **are** source breaking for consumers who have nullable reference types enabled themselves:
those projects will see new warnings until they adjust.

## Members that became nullable

Consumers must now handle null (or will see `CS8600`/`CS8602` where they previously saw nothing).

| Member | Was | Now | Why |
| --- | --- | --- | --- |
| `Resolver.Version` | `string` | `string?` | Reads `AssemblyInformationalVersionAttribute` through `?.`, which yields null when the attribute is absent. |
| `Record.RecordData` | `List<byte>` | `List<byte>?` | Never assigned anywhere in the library — see "Known issues" below. |

## Members that now accept null explicitly

These already handled null at runtime; the signature now says so. Consumers gain flexibility and
lose nothing.

| Member | Was | Now |
| --- | --- | --- |
| `Question.Equals(Question)` | `Question` | `[NotNullWhen(true)] Question?` |
| `Question.Equals(object)` | `object` | `[NotNullWhen(true)] object?` |
| `RecordSrv.CompareTo(RecordSrv)` | `RecordSrv` | `RecordSrv?` |

The `[NotNullWhen(true)]` attributes let callers skip a redundant null check after a `true` result.

## Everything else is now non-nullable

Enabling the feature implicitly declares every other unannotated reference type in the public
surface as non-nullable. The practical effect: passing `null` to a method such as
`Resolver.Query(string domainName, ...)` or `ResolverBuilder.AddDnsServer(string serverAddress)`
now produces a compiler warning at the call site.

This matches the behaviour that was already there — those paths threw or silently no-opped on null
before. Nothing changed at runtime; the contract is simply now visible to the compiler.

## Known issues surfaced by the migration, not fixed here

The migration was strictly an annotation exercise and changed no runtime behaviour. Two things it
uncovered are worth addressing separately:

- **`Record.RecordData` is dead API.** It is declared with a getter only, is never assigned by any
  constructor or derived record, and is never read anywhere in the library. It is therefore always
  `null`. It is annotated `List<byte>?` here to be truthful, but the real fix is to remove it — a
  genuine breaking change that deserves its own release note.
- **`Response(IPEndPoint server, byte[] data)` does not null-check `server`.** It validates `data`
  with `ArgumentNullException.ThrowIfNull` but assigns `server` unchecked, so a null argument
  leaves the non-nullable `Response.Server` property holding null, contradicting its annotation.
  Adding the guard is a runtime behaviour change and was deliberately left out of this pass.
