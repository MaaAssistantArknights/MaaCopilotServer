// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
///     The service for parsing information of the current user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    ///     Gets user identity (GUID) of the current user.
    /// </summary>
    /// <returns>User GUID if it exists, otherwise <c>null</c>.</returns>
    Guid? GetUserIdentity();

    /// <summary>
    ///     Get <see cref="CopilotUser"/> instance of current user. It will return <c>null</c> if user is not logged in.
    /// </summary>
    /// <returns><see cref="CopilotUser"/> instance or <c>null</c> if user is not logged in.</returns>
    Task<Domain.Entities.CopilotUser?> GetUser();

    /// <summary>
    ///     Gets tracking ID of the current user. The tracking ID follows the rules below:
    ///     <para>When APM is enabled, the ID will be APM Tracking ID.</para>
    ///     <para>When APM is disabled, the ID will be <see cref="HttpContext.TraceIdentifier" /> provided by ASP.NET Core.</para>
    /// </summary>
    /// <returns>The tracking ID if it exists, otherwise empty string.</returns>
    string GetTrackingId();

    /// <summary>
    ///     Get the culture of the current user.
    ///     <para>The default culture is zh-CN.</para>
    /// </summary>
    /// <returns>The <see cref="CultureInfo"/> instance.</returns>
    CultureInfo GetCulture();
}
