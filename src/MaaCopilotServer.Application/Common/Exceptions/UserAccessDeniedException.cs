// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.Common.Exceptions;

public class UserAccessDeniedException : Exception
{
    public UserAccessDeniedException(UserRole currentUserRole, UserRole requiredUserRole)
        : base($"User with role \"{currentUserRole}\" cannot access resource with role \"{requiredUserRole}\" required") { }
}
