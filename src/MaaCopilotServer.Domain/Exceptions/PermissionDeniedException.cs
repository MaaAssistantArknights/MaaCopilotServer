// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Exceptions;

/// <summary>
/// Permission denied exception
/// </summary>
public class PermissionDeniedException : Exception
{
    public PermissionDeniedException(UserPermission required, UserPermission current)
        : base($"Permission denied. Required: \"{required}\", current: \"{current}\"") { }
}
