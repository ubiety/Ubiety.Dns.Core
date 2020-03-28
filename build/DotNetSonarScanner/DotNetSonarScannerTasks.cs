using Nuke.Common.Tooling;

namespace Nuke.Common.Tools.DotNetSonarScanner
{
    partial class DotNetSonarScannerTasks
    {
        internal static string GetToolPath(string framework = null)
        {
            return ToolPathResolver.GetPackageExecutable(
                "dotnet-sonarscanner|MSBuild.SonarQube.Runner.Tool",
                "SonarScanner.MSBuild.dll|SonarScanner.MSBuild.exe",
                framework: framework);
        }
    }
}