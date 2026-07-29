# Breaking changes — consumer impact

Public API changes in `Ubiety.Dns.Core` that consumers should know about before upgrading.

## Removed members

Three public members were removed. All were unreachable dead API: nothing in the library assigned,
raised or referenced them, and no consumer could have used them for their stated purpose.

| Member | Dead since | Why |
| --- | --- | --- |
| `Ubiety.Dns.Core.Common.VerboseEventArgs` | 2020-03-27 | Payload for `Resolver.OnVerbose`. Commit `af2cd30` removed the event, the `VerboseEventHandler` delegate, the `Verbose()` method and all four call sites, but left the payload class behind. Logging moved to `IUbietyLogger`. |
| `Ubiety.Dns.Core.Common.VerboseOutputEventArgs` | never used | Declared in the original 2008 import at `Resolver.cs:130` while the event on line 148 used `VerboseEventArgs`. An unused duplicate carried forward through every refactor since. |
| `Ubiety.Dns.Core.Records.Record.RecordData` | 2020-03-29 | Commit `43ea6ad` removed the `RecordData = new List<byte>(Reader.ReadBytes(length))` assignment because reading those bytes fast-forwarded the reader and broke parsing. The property stayed declared and returned `null` on every record from then on. |

These removals **are binary breaking**: code compiled against an earlier version that names any of
these types or members will fail to load. In practice nothing could have depended on them — the
library declares no `event` members at all, so there was nothing to hand the event arguments to, and
`RecordData` was always `null`.

## Nullable reference type annotations

`Ubiety.Dns.Core` now builds with `<Nullable>enable</Nullable>`. Nullable annotations are part of
the public API contract, so this section records what changed for consumers.

None of the annotation changes are **binary** breaking changes — no value type gained `Nullable<T>`, and no signature
changed in a way that affects the emitted metadata beyond nullability attributes. Existing compiled
assemblies keep working without recompilation.

They **are** source breaking for consumers who have nullable reference types enabled themselves:
those projects will see new warnings until they adjust.

### Members that became nullable

Consumers must now handle null (or will see `CS8600`/`CS8602` where they previously saw nothing).

| Member | Was | Now | Why |
| --- | --- | --- | --- |
| `Resolver.Version` | `string` | `string?` | Reads `AssemblyInformationalVersionAttribute` through `?.`, which yields null when the attribute is absent. |

### Members that now accept null explicitly

These already handled null at runtime; the signature now says so. Consumers gain flexibility and
lose nothing.

| Member | Was | Now |
| --- | --- | --- |
| `Question.Equals(Question)` | `Question` | `[NotNullWhen(true)] Question?` |
| `Question.Equals(object)` | `object` | `[NotNullWhen(true)] object?` |
| `RecordSrv.CompareTo(RecordSrv)` | `RecordSrv` | `RecordSrv?` |
| `Question.operator ==` | `(Question, Question)` | `(Question?, Question?)` |
| `Question.operator !=` | `(Question, Question)` | `(Question?, Question?)` |

The two operators were missed in the original pass and caught later by a test that compared a
`Question` against `null`. They delegate to `Equals(left, right)`, which has always handled null
correctly, so only the annotation was wrong: callers were warned off a comparison that works.

The `[NotNullWhen(true)]` attributes let callers skip a redundant null check after a `true` result.

### Everything else is now non-nullable

Enabling the feature implicitly declares every other unannotated reference type in the public
surface as non-nullable. The practical effect: passing `null` to a method such as
`Resolver.Query(string domainName, ...)` or `ResolverBuilder.AddDnsServer(string serverAddress)`
now produces a compiler warning at the call site.

This matches the behaviour that was already there — those paths threw or silently no-opped on null
before. Nothing changed at runtime; the contract is simply now visible to the compiler.

## Added members

Two overloads of `Resolver.QueryAsync` return `Task<Response>`:

- `QueryAsync(string, QuestionType, CancellationToken)` for the common internet class case
- `QueryAsync(string, QuestionType, QuestionClass = IN, CancellationToken = default)`

Both are additive: the synchronous `Query` is unchanged and remains supported. The overloads cannot
be ambiguous, since a `CancellationToken` does not convert to a `QuestionClass`.

The async path is implemented independently down to the socket rather than wrapping the synchronous
one, so it neither blocks a thread nor risks the deadlock that `.Result` over an async call causes.
Cancellation and timeout are deliberately distinct: a cancelled token throws
`OperationCanceledException` and abandons the query, while an elapsed timeout fails over to the next
server and ultimately returns a `Response` with `TimedOut` set, matching what a synchronous query
does.

## Behaviour changes

- **`Response(IPEndPoint server, byte[] data)` now rejects a null `server`.** It previously
  validated only `data` and assigned `server` unchecked, so a null argument left the non-nullable
  `Response.Server` property holding null and contradicting its own annotation. It now throws
  `ArgumentNullException` at the constructor, in line with every other public entry point in the
  library. No code path inside the library is affected: both internal call sites take their server
  from the resolver's configured list.

Both issues the nullable migration originally recorded here are now resolved: `Record.RecordData`
was removed, see the removals above, and the missing guard is the change described in this section.
