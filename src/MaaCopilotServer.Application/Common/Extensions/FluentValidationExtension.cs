// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Extensions;

public static class FluentValidationExtension
{
    public static bool BeValidGuid(string? id)
    {
        var isGuid = Guid.TryParse(id, out _);
        return isGuid;
    }
}
