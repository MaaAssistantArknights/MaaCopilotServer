// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Test.TestEntities;

/// <summary>
/// The factory class of <see cref="CopilotUser"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotUserFactory : ITestEntityFactory<CopilotUser>
{
    /// <summary>
    ///     The email of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     The password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     The username of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     The role of the user.
    /// </summary>
    public UserRole UserRole { get; set; } = UserRole.User;

    /// <summary>
    ///     Creator GUID
    /// </summary>
    public Guid? CreateBy { get; set; } = Guid.Empty;

    /// <inheritdoc/>
    public CopilotUser Build()
    {
        return new CopilotUser(Email, Password, UserName, UserRole, CreateBy);
    }
}
