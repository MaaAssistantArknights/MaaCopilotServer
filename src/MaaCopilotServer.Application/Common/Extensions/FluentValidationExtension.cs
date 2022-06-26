// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;

namespace MaaCopilotServer.Application.Common.Extensions;

/// <summary>
///     Common functions to validate values.
/// </summary>
public static class FluentValidationExtension
{
    /// <summary>
    ///     Checks if the GUID given is a valid GUID.
    /// </summary>
    /// <param name="id">The GUID.</param>
    /// <returns><c>true</c> if the GUID is valid; <c>false</c> otherwise.</returns>
    public static bool BeValidGuid(string? id)
    {
        var isGuid = Guid.TryParse(id, out _);
        return isGuid;
    }

    /// <summary>
    ///     Validates the content to ensure it has <c>stage_name</c> and <c>minimum_required</c> fields.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns><c>true</c> if the content is valid, <c>false</c> otherwise.</returns>
    public static bool BeValidatedOperationContent(string? content)
    {
        try
        {
            var doc = JsonDocument.Parse(content!).RootElement;
            var stageName = doc.GetProperty("stage_name").GetString();
            var minimumRequired = doc.GetProperty("minimum_required").GetString();
            return string.IsNullOrEmpty(stageName) is false && string.IsNullOrEmpty(minimumRequired) is false;
        }
        catch
        {
            return false;
        }
    }
}
