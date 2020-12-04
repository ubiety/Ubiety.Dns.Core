/*
 *      Copyright (C) 2020 Dieter (coder2000) Lunn
 *
 *      This program is free software: you can redistribute it and/or modify
 *      it under the terms of the GNU General Public License as published by
 *      the Free Software Foundation, either version 3 of the License, or
 *      (at your option) any later version.
 *
 *      This program is distributed in the hope that it will be useful,
 *      but WITHOUT ANY WARRANTY; without even the implied warranty of
 *      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *      GNU General Public License for more details.
 *
 *      You should have received a copy of the GNU General Public License
 *      along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.SonarScanner;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.SonarScanner.SonarScannerTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] readonly bool? Cover = true;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion(Framework = "netcoreapp3.1")] readonly GitVersion GitVersion;
    [Parameter] readonly string NuGetKey;

    const string NuGetSource = "https://api.nuget.org/v3/index.json";

    [Solution] readonly Solution Solution;

    [Parameter] readonly string SonarKey;
    const string SonarProjectKey = "ubiety_Ubiety.Dns.Core";

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target SonarBegin => _ => _
        .Before(Compile)
        .Requires(() => SonarKey)
        .Unlisted()
        .Executes(() =>
        {
            SonarScannerBegin(s => s
                .SetLogin(SonarKey)
                .SetProjectKey(SonarProjectKey)
                .SetServer("https://sonarcloud.io")
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetOpenCoverPaths(ArtifactsDirectory / "coverage.opencover.xml")
                .SetProcessArgumentConfigurator(args => args.Add("/o:ubiety"))
                .SetFramework("net5.0"));
        });

    Target SonarEnd => _ => _
        .After(Test)
        .DependsOn(SonarBegin)
        .Requires(() => SonarKey)
        .AssuredAfterFailure()
        .Unlisted()
        .Executes(() =>
        {
            SonarScannerEnd(s => s
                .SetLogin(SonarKey)
                .SetFramework("net5.0"));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution.GetProject("Ubiety.Dns.Test"))
                .EnableNoBuild()
                .SetConfiguration(Configuration)
                .SetProcessArgumentConfigurator(args => args.Add("/p:CollectCoverage={0}", Cover)
                    .Add("/p:CoverletOutput={0}", ArtifactsDirectory / "coverage")
                    .Add("/p:CoverletOutputFormat={0}", "opencover")
                    .Add("/p:Exclude={0}", "[xunit.*]*")));
        });

    Target Pack => _ => _
        .After(Test)
        .OnlyWhenStatic(() => GitRepository.Branch == "main")
        .Executes(() =>
        {
            DotNetPack(s => s
                .EnableNoBuild()
                .SetConfiguration(Configuration)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetVersion(GitVersion.NuGetVersionV2));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Requires(() => NuGetKey)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .OnlyWhenStatic(() => GitRepository.Branch == "main")
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                    .SetApiKey(NuGetKey)
                    .SetSource(NuGetSource)
                    .CombineWith(
                        ArtifactsDirectory.GlobFiles("*.nupkg").NotEmpty(), (cs, v) =>
                            cs.SetTargetPath(v)),
                5,
                true);
        });

    Target Appveyor => _ => _
        .DependsOn(Test, SonarEnd, Publish);

    public static int Main() => Execute<Build>(x => x.Test);
}
