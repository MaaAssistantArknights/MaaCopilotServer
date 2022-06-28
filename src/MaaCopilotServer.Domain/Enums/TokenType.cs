// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Enums;

/// <summary>
///     Enumeration of the possible types of the Token.
/// </summary>
public enum TokenType
{
    /// <summary>
    ///     User account activation token.
    /// </summary>
    UserActivation = 0,

    /// <summary>
    ///     User password reset token.
    /// </summary>
    UserPasswordReset = 1
}
