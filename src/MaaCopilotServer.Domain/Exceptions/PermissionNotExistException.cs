// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Exceptions;

/// <summary>
/// Permission code not exist exception
/// </summary>
public class PermissionNotExistException : Exception
{
    public PermissionNotExistException(string permission)
        : base($"Permission \"{permission}\" does not exist.") { }
}
