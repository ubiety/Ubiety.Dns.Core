/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.GitHubActions.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

namespace _build;

/// <summary>
///     Generates the workflow like <see cref="GitHubActionsAttribute" /> does, plus a NuGet
///     trusted publishing login step whose short-lived key is handed to the build invocation.
/// </summary>
/// <remarks>
///     The attribute has no hook for arbitrary steps, so the login has to be injected into the
///     generated job. Keeping it here means the workflow survives regeneration.
/// </remarks>
class CustomGitHubActionsAttribute(string name, GitHubActionsImage image, params GitHubActionsImage[] images)
    : GitHubActionsAttribute(name, image, images)
{
    protected override GitHubActionsJob GetJobs(
        GitHubActionsImage image,
        IReadOnlyCollection<ExecutableTarget> relevantTargets)
    {
        var job = base.GetJobs(image, relevantTargets);

        // The key is only valid for an hour, so login goes as late as possible - directly ahead
        // of the invocation that pushes.
        var steps = job.Steps.ToList();
        var index = steps.FindIndex(x => x is GitHubActionsRunStep);
        steps.Insert(index >= 0 ? index : steps.Count, new NuGetLoginStep());
        job.Steps = steps.ToArray();

        return job;
    }

    protected override IEnumerable<(string, string)> GetImports()
    {
        return base.GetImports().Concat([("NUGETKEY", $"${{{{ steps.{NuGetLoginStep.StepId}.outputs.NUGET_API_KEY }}}}")]);
    }
}

/// <summary>
///     Exchanges the job's OIDC token for a short-lived nuget.org API key.
/// </summary>
class NuGetLoginStep : GitHubActionsStep
{
    public const string StepId = "nuget-login";

    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine("- name: 'NuGet Login'");

        using (writer.Indent())
        {
            writer.WriteLine("uses: NuGet/login@v1");
            writer.WriteLine($"id: {StepId}");
            writer.WriteLine("with:");

            using (writer.Indent())
            {
                writer.WriteLine("user: ${{ secrets.NUGET_USERNAME }}");
            }
        }
    }
}
