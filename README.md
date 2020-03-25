# ![Logo](https://github.com/ubiety/Ubiety.Dns.Core/raw/develop/images/library64.png) Ubiety.Dns.Core ![Nuget](https://img.shields.io/nuget/v/Ubiety.Dns.Core.svg?style=flat-square)

> A reusable DNS resolver for .NET Standard 2.0

Thank you to the initial work of Alphons van der Heijden and Geoffry Huntley on this library.

| Branch  | Quality                                                                                                                                                                                                   | Travis CI                                                                                                                                                   | Appveyor                                                                                                                                                                                   | Coverage                                                                                                                                                        |
| ------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Master  | [![CodeFactor](https://www.codefactor.io/repository/github/ubiety/ubiety.dns.core/badge?style=flat-square)](https://www.codefactor.io/repository/github/ubiety/ubiety.dns.core)                           | [![Travis (.org) branch](https://img.shields.io/travis/ubiety/Ubiety.Dns.Core/master.svg?style=flat-square)](https://travis-ci.org/ubiety/Ubiety.Dns.Core)  | [![AppVeyor branch](https://img.shields.io/appveyor/ci/coder2000/ubiety-dns-core/master.svg?style=flat-square)](https://ci.appveyor.com/project/coder2000/ubiety-dns-core/branch/master)   | [![Codecov branch](https://img.shields.io/codecov/c/github/ubiety/Ubiety.Dns.Core/master.svg?style=flat-square)](https://codecov.io/gh/ubiety/Ubiety.Dns.Core)  |
| Develop | [![CodeFactor](https://www.codefactor.io/repository/github/ubiety/ubiety.dns.core/badge/develop?style=flat-square)](https://www.codefactor.io/repository/github/ubiety/ubiety.xmpp.core/overview/develop) | [![Travis (.org) branch](https://img.shields.io/travis/ubiety/Ubiety.Dns.Core/develop.svg?style=flat-square)](https://travis-ci.org/ubiety/Ubiety.Dns.Core) | [![AppVeyor branch](https://img.shields.io/appveyor/ci/coder2000/ubiety-dns-core/develop.svg?style=flat-square)](https://ci.appveyor.com/project/coder2000/ubiety-dns-core/branch/develop) | [![Codecov branch](https://img.shields.io/codecov/c/github/ubiety/Ubiety.Dns.Core/develop.svg?style=flat-square)](https://codecov.io/gh/ubiety/Ubiety.Dns.Core) |

## Installing / Getting started

Ubiety DNS Core is available from NuGet

```shell
dotnet package install Ubiety.Dns.Core
```

You can also use your favorite NuGet client.

## Developing

Here's a brief intro about what a developer must do in order to start developing
the project further:

```shell
git clone https://github.com/ubiety/Ubiety.Dns.Core.git
cd Ubiety.Dns.Core
dotnet restore
```

Clone the repository and then restore the development requirements. You can use
any editor, Rider, VS Code or VS 2017. The library supports all .NET Core
platforms.

### Building

Building is simple

```shell
./build.ps1
```

### Deploying / Publishing

```shell
git pull
versionize
dotnet pack
dotnet nuget push
git push
```

## Contributing

When you publish something open source, one of the greatest motivations is that
anyone can just jump in and start contributing to your project.

These paragraphs are meant to welcome those kind souls to feel that they are
needed. You should state something like:

"If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are warmly welcome."

If there's anything else the developer needs to know (e.g. the code style
guide), you should link it here. If there's a lot of things to take into
consideration, it is common to separate this section to its own file called
`CONTRIBUTING.md` (or similar). If so, you should say that it exists here.

## Links

- Project homepage: <https://dns.ubiety.ca>
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
