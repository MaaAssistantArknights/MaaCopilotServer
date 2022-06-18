// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.Common.Security;

/// <summary>
///     The attribute for authorization on user roles.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AuthorizedAttribute : Attribute
{
    /// <summary>
    ///     The constructor of <see cref="AuthorizedAttribute" />.
    /// </summary>
    /// <param name="role">The role of the user that is required.</param>
    /// <param name="allowInActivated">Allow inactivated account access.</param>
    public AuthorizedAttribute(UserRole role, bool allowInActivated = false)
    {
        Role = role;
        AllowInActivated = allowInActivated;
    }

    /// <summary>
    ///     The role of the user that is required.
    /// </summary>
    public UserRole Role { get; }

    /// <summary>
    ///     Allow inactivated account access.
    /// </summary>
    public bool AllowInActivated { get; set; }
}
