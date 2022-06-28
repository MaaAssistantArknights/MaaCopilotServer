// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Email.Models;

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
///     Interface for the email service.
/// </summary>
public interface IMailService
{
    /// <summary>
    ///     Send an email.
    /// </summary>
    /// <param name="model">The email model.</param>
    /// <param name="targetAddress">The receiver's email address.</param>
    /// <typeparam name="T">Email model type, must inherit from <see cref="IEmailModel"/></typeparam>
    /// <returns>A task with bool result to indicated if the email has been sent successfully.</returns>
    Task<bool> SendEmailAsync<T>(T model, string targetAddress) where T : class, IEmailModel;
}
