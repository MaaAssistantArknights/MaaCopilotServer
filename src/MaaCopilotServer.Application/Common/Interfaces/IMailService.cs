// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Email.Models;

namespace MaaCopilotServer.Application.Common.Interfaces;

public interface IMailService
{
    Task<bool> SendEmailAsync<T>(T model, string targetAddress) where T : class, IEmailModel;
}
