// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Test.TestHelpers;
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
    public string Email { get; set; } = HandlerTest.TestEmail;

    /// <summary>
    ///     The password of the user.
    /// </summary>
    public string Password { get; set; } = HandlerTest.TestHashedPassword;

    /// <summary>
    ///     The username of the user.
    /// </summary>
    public string UserName { get; set; } = HandlerTest.TestUsername;

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