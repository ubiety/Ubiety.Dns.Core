#addin "nuget:?package=Cake.Sonar"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Sonar");
var configuration = Argument("configuration", "Debug");

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

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        DotNetCoreClean(projectDir);
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
    .IsDependentOn("Clean")
    .Does(() => {
        DotNetCoreBuild(projectDir);
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);