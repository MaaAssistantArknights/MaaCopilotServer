// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.Common.Security;

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizedAttribute : Attribute
{
    public UserRole Role { get; }

    public AuthorizedAttribute(UserRole role)
    {
        Role = role;
    }
}
