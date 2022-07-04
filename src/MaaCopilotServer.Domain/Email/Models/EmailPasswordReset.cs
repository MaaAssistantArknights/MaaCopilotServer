// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Email.Models;

/// <summary>
///     User password reset email model.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="Token">The activation code.</param>
/// <param name="ValidBefore">The time that the token is valid before.</param>
/// <param name="HasCallbackUrl">The email has callback url or not.</param>
[EmailTemplate("EmailPasswordReset")]
[ExcludeFromCodeCoverage]
public record EmailPasswordReset(string UserName, string Token, string ValidBefore, bool HasCallbackUrl) : IEmailModel;
