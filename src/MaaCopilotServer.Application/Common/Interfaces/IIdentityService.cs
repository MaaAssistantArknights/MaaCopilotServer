// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
///     The service for getting user info.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    ///     Gets user info asynchronously.
    /// </summary>
    /// <param name="guid">The GUID of the user.</param>
    /// <returns>A task with the user info if it exists, otherwise <c>null</c>.</returns>
    Task<Domain.Entities.CopilotUser?> GetUserAsync(Guid guid);
}
