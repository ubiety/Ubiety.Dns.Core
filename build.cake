#addin "nuget:?package=Cake.Sonar"
#addin "Cake.MiniCover"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"
#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=xunit.runner.console"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Coverage");
var configuration = Argument("configuration", "Debug");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

SetMiniCoverToolsProject("./tools/tools.csproj");


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

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        DotNetCoreClean(projectDir);
    });

Task("UpdateVersion")
    .IsDependentOn("Clean")
    .Does(() => {
        GitVersion(new GitVersionSettings{
            UpdateAssemblyInfo = true
        });
    });

Task("Pack")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCorePack(projectDir);
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        var testAssemblies = GetFiles("./test/**/bin/Debug/netcoreapp2.0/Ubiety.Dns.Test.dll");
        XUnit2(testAssemblies);
    });

Task("Coverage")
    .IsDependentOn("Build")
    .Does(() => {
        MiniCover(tool => {
            foreach (var project in GetFiles("./test/**/*.csproj"))
            {
                tool.DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings{
                    NoBuild = true,
                    Configuration = configuration
                });
            }
        }, new MiniCoverSettings()
            .WithAssembliesMatching("./test/**/*.dll")
            .WithSourcesMatching("./src/**/*.cs")
            .GenerateReport(ReportType.XML));
    });

Task("SonarBegin")
    .Does(() => {
        SonarBegin(new SonarBeginSettings{
            Url = "https://sonarcloud.io",
            Key = "dns",
            Organization = "coder2000-github",
            Login = "6a7700a6bfbe29e25e38e7996631c142ef24480a"
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
    .IsDependentOn("SonarEnd");

Task("Build")
    .IsDependentOn("UpdateVersion")
    .Does(() => {
        DotNetCoreBuild(projectDir);
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);