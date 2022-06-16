// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Interfaces;

using Microsoft.AspNetCore.Http;

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
    /// Gets tracking ID of the current user. The tracking ID follows the rules below:
    /// 
    /// <para>When APM is enabled, the ID will be APM Tracking ID.</para>
    /// 
    /// <para>When APM is disabled, the ID will be <see cref="HttpContext.TraceIdentifier"/> provided by ASP.NET Core.</para>
    /// </summary>
    /// <returns>The tracking ID if it exists, otherwise empty string.</returns>
    string GetTrackingId();
}
