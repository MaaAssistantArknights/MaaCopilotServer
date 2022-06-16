// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Email.Models;

/// <summary>
///     User password reset email model.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="Token">The activation code.</param>
/// <param name="ValidBefore">The time that the token is valid before.</param>
[EmailTemplate("EmailPasswordReset")]
public record EmailPasswordReset(string UserName, string Token, string ValidBefore) : IEmailModel;
