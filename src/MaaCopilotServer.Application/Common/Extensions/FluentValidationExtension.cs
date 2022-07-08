// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

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
}
