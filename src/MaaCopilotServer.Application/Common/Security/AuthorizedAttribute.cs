// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.Common.Security;

/// <summary>
/// The attribute for authorization on user roles.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AuthorizedAttribute : Attribute
{
    /// <summary>
    /// The constructor of <see cref="AuthorizedAttribute"/>.
    /// </summary>
    /// <param name="role">The role of the user.</param>
    public AuthorizedAttribute(UserRole role)
    {
        Role = role;
    }

    /// <summary>
    /// The role of the user.
    /// </summary>
    public UserRole Role { get; }
}
