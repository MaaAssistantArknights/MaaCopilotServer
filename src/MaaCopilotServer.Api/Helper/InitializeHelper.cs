// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
///     The helper class of database connection.
/// </summary>
[ExcludeFromCodeCoverage]
public static class InitializeHelper
{
    /// <summary>
    /// Initializes email templates.
    /// </summary>
    public static void InitializeEmailTemplates()
    {
        var originalTemplatesDirectory = new DirectoryInfo(GlobalConstants.OriginalTemplatesDirectory);
        var targetTemplatesDirectory = new DirectoryInfo(GlobalConstants.TargetTemplatesDirectory);

        if (targetTemplatesDirectory.Exists is false)
        {
            targetTemplatesDirectory.Create();
        }

        var originalTemplates = originalTemplatesDirectory.GetFiles();
        var targetTemplates = targetTemplatesDirectory.GetFiles();

        foreach (var originalTemplate in originalTemplates)
        {
            if (targetTemplates.Any(x => x.Name == originalTemplate.Name) == false)
            {
                originalTemplate.CopyTo(targetTemplatesDirectory.FullName.CombinePath(originalTemplate.Name));
            }
        }
    }
}
