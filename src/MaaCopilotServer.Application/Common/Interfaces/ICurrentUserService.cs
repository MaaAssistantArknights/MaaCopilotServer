// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
/// The service for parsing information of the current user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets user identity (GUID) of the current user.
    /// </summary>
    /// <returns>User GUID if it exists, otherwise <c>null</c>.</returns>
    Guid? GetUserIdentity();

    /// <summary>
    /// Gets tracking ID of the current user.
    /// </summary>
    /// <returns>The tracking ID if it exists, otherwise empty string.</returns>
    string GetTrackingId();
}
