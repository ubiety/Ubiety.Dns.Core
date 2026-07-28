# Central Package Management + Directory.Build.props conversion

## 1. Conversion overview

**Scope:** the whole repository — all four projects.

| Project | PackageReferences centralized |
| --- | --- |
| `src/Ubiety.Dns.Core` | 3 |
| `tests/Ubiety.Dns.Test` | 9 |
| `src/Dns.Sample` | 1 |
| `build/_build` | 1 |

**Files added**

- `Directory.Packages.props` — `ManagePackageVersionsCentrally=true` plus 14 `PackageVersion` entries.
- `Directory.Build.props` — shared `TargetFramework`, `LangVersion`, `Nullable`, `WarningsAsErrors`
  and a default of `IsPackable=false`.

**Not centralized**

- The two `PackageDownload` items in `build/_build.csproj` (`GitVersion.Tool`,
  `dotnet-sonarscanner`). `PackageDownload` is not governed by CPM and keeps its inline version
  ranges.

**Properties removed as redundant**

`Ubiety.Dns.Core.csproj` declared `AnalysisLevel` and `EnableNETAnalyzers` **twice each** in the
same `PropertyGroup`. Both were deleted rather than moved: evaluating every project confirmed
`AnalysisLevel=latest` and `EnableNETAnalyzers=true` are already the .NET 10 SDK defaults, including
in the three projects that never set them. The effective values are unchanged.

## 2. Version conflict resolutions

**No conflicts.** Every one of the 14 packages is referenced by exactly one project, so there was
no version to reconcile and no decision to make. Each `PackageVersion` entry is simply the version
that project already used.

## 3. Package comparison — baseline vs. result

Comparing `baseline-packages.json` against `after-cpm-packages.json`, keyed on
(project, package):

**Changes: none.**

All 14 entries resolve to the identical version they did before the conversion. The change is fully
version-neutral.

| Project | Package | Version (unchanged) |
| --- | --- | --- |
| Ubiety.Dns.Core | Stylecop.Analyzers | 1.2.0-beta.435 |
| Ubiety.Dns.Core | Ubiety.Logging.Core | 1.2.2 |
| Ubiety.Dns.Core | Ubiety.StyleCop | 0.0.5 |
| Dns.Sample | System.CommandLine | 2.0.10 |
| _build | Nuke.Common | 9.0.4 |
| Ubiety.Dns.Test | coverlet.msbuild | 6.0.4 |
| Ubiety.Dns.Test | Microsoft.NET.Test.Sdk | 17.13.0 |
| Ubiety.Dns.Test | Moq | 4.20.72 |
| Ubiety.Dns.Test | Moq.Analyzers | 0.3.0 |
| Ubiety.Dns.Test | Roslynator.Analyzers | 4.13.1 |
| Ubiety.Dns.Test | Shouldly | 4.3.0 |
| Ubiety.Dns.Test | xunit | 2.9.3 |
| Ubiety.Dns.Test | xunit.analyzers | 1.20.0 |
| Ubiety.Dns.Test | xunit.runner.visualstudio | 3.0.2 |

## 4. Property comparison — baseline vs. result

Package versions are only half the story when `Directory.Build.props` is introduced, so evaluated
MSBuild properties were captured before and after (`baseline-properties.txt`,
`after-properties.txt`). Four differences, all understood:

### 4.1 `NU1605` is now escalated to an error in three more projects

| Project | Before | After |
| --- | --- | --- |
| Ubiety.Dns.Core | `nullable;SYSLIB0011` | `nullable;NU1605;SYSLIB0011` |
| Ubiety.Dns.Test | `nullable;SYSLIB0011` | `nullable;NU1605;SYSLIB0011` |
| Dns.Sample | `nullable;SYSLIB0011` | `nullable;NU1605;SYSLIB0011` |
| _build | `;NU1605;SYSLIB0011` | `nullable;NU1605;SYSLIB0011` |

This is **not** caused by CPM. The .NET 10 SDK appends `NU1605` and `SYSLIB0011` to
`WarningsAsErrors` for every project — a bare `dotnet new classlib` with no other files evaluates to
`;NU1605;SYSLIB0011`.

The old layout set `<WarningsAsErrors>nullable</WarningsAsErrors>` in the project body, which is
evaluated **after** the SDK props, so the plain assignment **overwrote** the `NU1605` the SDK had
already accumulated. `Directory.Build.props` is evaluated **before** the SDK props, so `nullable`
becomes the seed value and the SDK appends its codes on top.

Net effect: `NU1605` (package downgrade detected) is treated as an error again in the three projects
that were previously discarding it. This makes the build stricter, and it can only fail a build that
genuinely contains a package downgrade. `_build` additionally picks up `nullable`, which is inert
there because it runs in `Nullable=annotations` mode where nullable warnings are disabled.

### 4.2 `Ubiety.Dns.Test` LangVersion: `14.0` → `latest`

The test project never set `LangVersion`, so it took the SDK default of `14.0`. The other three
projects all set `latest` explicitly, and the shared file adopts that. On the current SDK the two
are the same language version, so nothing changes today. Worth knowing that `latest` floats: on a
future SDK it will track the newest C# the compiler supports rather than the TFM default.

### 4.3 `Dns.Sample` IsPackable: `False` → `false`

Casing only. MSBuild booleans are case-insensitive; no functional change.

### 4.4 Everything else identical

`TargetFramework`, `Nullable`, `AnalysisLevel`, `EnableNETAnalyzers`, `SignAssembly` and
`DocumentationFile` are byte-identical across all four projects.

## 5. Risk assessment

**[Low risk]** — The conversion is version-neutral: all 14 packages resolve exactly as before, a
clean solution build succeeds with 0 errors and the same 43 pre-existing StyleCop warnings, and all
38 tests pass.

The one behavioural change worth reviewing is 4.1: three projects now fail the build on a package
downgrade instead of warning. That is the SDK's intended default and the previous behaviour was an
accident of property ordering, but it is a real change in strictness.

## 6. Follow-up items

1. **Security advisory (high severity).** `System.Security.Cryptography.Xml` 8.0.0 arrives
   transitively through `Nuke.Common` 9.0.4 in `build/_build.csproj` and has three known high
   severity advisories: [GHSA-g8r8-53c2-pm3f](https://github.com/advisories/GHSA-g8r8-53c2-pm3f),
   [GHSA-mmjf-rqrv-855v](https://github.com/advisories/GHSA-mmjf-rqrv-855v) and
   [GHSA-w3x6-4m5h-cxqf](https://github.com/advisories/GHSA-w3x6-4m5h-cxqf). This affects the build
   tooling only, not the shipped package. Upgrading was deliberately left out of this conversion to
   keep it version-neutral.
2. **Outdated test dependencies.** `coverlet.msbuild` 6.0.4 → 10.0.1, `Microsoft.NET.Test.Sdk`
   17.13.0 → 18.8.1, `Roslynator.Analyzers` 4.13.1 → 4.15.0, `xunit.analyzers` 1.20.0 → 1.27.0,
   `xunit.runner.visualstudio` 3.0.2 → 3.1.5, `Moq.Analyzers` 0.3.0 → 0.4.2. Now a one-file change
   in `Directory.Packages.props`.
3. **Decide on `LangVersion`.** All four projects now float on `latest`. Pinning to the TFM default
   instead would make builds more reproducible across SDK versions.
4. **`_build` is in the solution but never built by it.** It has `ActiveCfg` entries with no
   `Build.0` entries, which is why `dotnet package list` initially failed on a missing assets file
   and it needs a separate `dotnet restore`. Intentional for a Nuke build project, but worth knowing.

## 7. Artifacts

All under `.cpm-artifacts/` (gitignored; safe to delete once reviewed):

| File | Purpose |
| --- | --- |
| `baseline.binlog` / `after-cpm.binlog` | MSBuild binary logs before and after, for manual inspection |
| `baseline-packages.json` / `after-cpm-packages.json` | Resolved package versions per project, source of the section 3 comparison |
| `baseline-properties.txt` / `after-properties.txt` | Evaluated MSBuild properties, source of the section 4 comparison |
