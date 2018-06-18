#addin "nuget:?package=Cake.Sonar"
#addin "nuget:?package=Cake.Codecov"
#tool "nuget:?package=Codecov"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"
#tool "nuget:?package=xunit.runner.console"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Debug");

var version = EnvironmentVariable("GitVersion_NuGetVersionV2");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

var projectDir = Directory("./src/Ubiety.Dns.Core");
var testDir = Directory("./test/Ubiety.Dns.Test");

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        DotNetCoreClean(projectDir);
        DotNetCoreClean(testDir);
    });

Task("Pack")
    .IsDependentOn("Sonar")
    .Does(() => {
        var ver = String.IsNullOrEmpty(version) ? "2.0" : version;
        DotNetCorePack(projectDir, new DotNetCorePackSettings{
            ArgumentCustomization = args => args.Append($"/p:PackageVersion={ver}")
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest("./test/Ubiety.Dns.Test/Ubiety.Dns.Test.csproj", new DotNetCoreTestSettings{
            ArgumentCustomization = args => args.Append("/p:CollectCoverage=true").Append("/p:CoverletOutputFormat=opencover"),
            NoBuild = true,
            Configuration = configuration
        });

        Codecov("test/Ubiety.Dns.Test/coverage.opencover.xml", "977eef75-3209-48ad-8543-92e88ccf4bc5");
    });

Task("SonarBegin")
    .Does(() => {
        SonarBegin(new SonarBeginSettings{
            Url = "https://sonarcloud.io",
            Key = "dns",
            Organization = "coder2000-github",
            Login = "6a7700a6bfbe29e25e38e7996631c142ef24480a",
            Version = version,
            OpenCoverReportsPath = "test/Ubiety.Dns.Test/coverage.opencover.xml"
        });
    });

Task("SonarEnd")
    .Does(() => {
        SonarEnd(new SonarEndSettings{
            Login = "6a7700a6bfbe29e25e38e7996631c142ef24480a"
        });
    });

Task("Sonar")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("SonarEnd");

Task("Build")
    .IsDependentOn("Clean")
    .Does(() => {
        DotNetCoreBuild(testDir);
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);