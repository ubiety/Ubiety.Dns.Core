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

using System.Collections.Generic;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.SonarScanner;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.SonarScanner.SonarScannerTasks;

namespace _build;

[GitHubActions("continuous",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.MacOsLatest,
    GitHubActionsImage.UbuntuLatest,
    OnPushBranchesIgnore = [ReleaseBranchPrefix, MasterBranch],
    OnPullRequestBranches = [DevelopBranch],
    PublishArtifacts = false,
    InvokedTargets = [nameof(Test), nameof(Publish)],
    EnableGitHubToken = true,
    FetchDepth = 0)]
[AppVeyor(
    AppVeyorImage.VisualStudioLatest,
    InvokedTargets = [nameof(Test), nameof(SonarEnd)],
    SkipTags = true,
    AutoGenerate = true)]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Required][GitRepository] readonly GitRepository GitRepository;
    [Required][GitVersion] readonly GitVersion GitVersion;
    [Required][Solution] readonly Solution Solution;

    [Parameter] readonly bool Cover = true;
    [Parameter] readonly string NuGetKey;
    [Parameter] readonly string GitHubToken;

    [CI] readonly GitHubActions GitHubActions;

    const string NuGetSource = "https://api.nuget.org/v3/index.json";
    string GitHubSource => $"https://nuget.pkg.github.com/{GitHubActions.RepositoryOwner}/index.json";

    bool Beta => GitRepository.IsOnDevelopBranch() || GitRepository.IsOnFeatureBranch();

    string Source => Beta ? GitHubSource : NuGetSource;
    string ApiKey => Beta ? GitHubToken : NuGetKey;

    const string SonarProjectKey = "ubiety_Ubiety.Dns.Core";

    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath TestsDirectory => RootDirectory / "tests";
    static AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    static AbsolutePath ChangelogFile => RootDirectory / "CHANGELOG.md";

    IEnumerable<AbsolutePath> PackageFiles => ArtifactsDirectory.GlobFiles("*.nupkg");


    const string MasterBranch = "main";
    const string DevelopBranch = "develop";
    const string ReleaseBranchPrefix = "release/*";

    [UsedImplicitly]
    Target Clean => t => t
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            TestsDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            ArtifactsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => t => t
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => t => t
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .SetNoRestore(InvokedTargets.Contains(Restore)));
        });

    Target SonarBegin => t => t
        .Before(Compile)
        .Unlisted()
        .Executes(() =>
        {
            SonarScannerBegin(s => s
                .SetProjectKey(SonarProjectKey)
                .SetServer("https://sonarcloud.io")
                .SetVersion(GitVersion.SemVer)
                .SetOpenCoverPaths(ArtifactsDirectory / "coverage.opencover.xml")
                .SetOrganization("ubiety")
                .SetFramework("net9.0"));
        });

    Target SonarEnd => t => t
        .After(Test)
        .DependsOn(SonarBegin)
        .AssuredAfterFailure()
        .Unlisted()
        .Executes(() =>
        {
            SonarScannerEnd(s => s
                .SetFramework("net9.0"));
        });

    Target Test => t => t
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution.GetProject("Ubiety.Dns.Test"))
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetConfiguration(Configuration)
                .When(Cover, c => c
                    .EnableCollectCoverage()
                    .SetCoverletOutput(ArtifactsDirectory / "coverage")
                    .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                    .SetProcessAdditionalArguments("/p:Exclude=[xunit.*]*")));
        });

    Target Pack => t => t
        .After(Test)
        .DependsOn(Compile)
        .Produces(ArtifactsDirectory / "*.nupkg")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .SetConfiguration(Configuration)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetVersion(GitVersion.SemVer));
        });

    Target Publish => t => t
        .DependsOn(Pack)
        .Consumes(Pack)
        .Requires(() => !NuGetKey.IsNullOrEmpty() || Beta)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .Executes(() =>
        {
            if (Beta)
            {
                DotNetNuGetAddSource(c => c
                    .SetSource(GitHubSource)
                    .SetUsername(GitHubActions.Actor)
                    .SetPassword(GitHubToken)
                    .SetStorePasswordInClearText(true));
            }

            DotNetNuGetPush(s => s
                    .SetApiKey(ApiKey)
                    .SetSource(Source)
                    .CombineWith(PackageFiles, (f, p) => f.SetTargetPath(p)),
                5,
                true);
        });

    public static int Main() => Execute<Build>(x => x.Test);
}
