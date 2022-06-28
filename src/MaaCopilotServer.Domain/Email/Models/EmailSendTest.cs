// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Email.Models;

/// <summary>
///     Test email model.
/// </summary>
/// <param name="Time">Current time string.</param>
[EmailTemplate("EmailSendTest")]
public record EmailSendTest(string Time) : IEmailModel;
