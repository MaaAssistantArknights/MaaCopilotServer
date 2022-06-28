// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.Common.Security;

/// <summary>
///     Helper class for authorization.
/// </summary>
public static class AuthorizationHelper
{
    /// <summary>
    ///     Checks if the user has the required permission.
    /// </summary>
    /// <param name="requestUser">The user make the request.</param>
    /// <param name="resourceUser">The owner of the resource.</param>
    /// <returns></returns>
    public static bool IsAllowAccess(this Domain.Entities.CopilotUser requestUser, Domain.Entities.CopilotUser resourceUser)
    {
        // Allow the owner to access
        if (requestUser.EntityId == resourceUser.EntityId)
        {
            return true;
        }

        // Allow admin to access none admin resource
        if (requestUser.UserRole >= UserRole.Admin && resourceUser.UserRole < UserRole.Admin)
        {
            return true;
        }

        // Allow super admin access everything
        if (requestUser.UserRole == UserRole.SuperAdmin)
        {
            return true;
        }

        return false;
    }
}
