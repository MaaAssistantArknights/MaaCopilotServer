// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Security.Claims;
using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Api.Services;

/// <summary>
/// The service for information of the current user.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    /// <summary>
    /// The HTTP context accessor.
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// The configuration.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// The constructor of <see cref="CurrentUserService"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="configuration">The configuration.</param>
    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    /// <summary>
    /// Gets user identity (GUID) of the current user.
    /// </summary>
    /// <returns>User GUID if it exists, otherwise <c>null</c>.</returns>
    public Guid? GetUserIdentity()
    {
        var id = _httpContextAccessor.HttpContext?.User.FindFirstValue("id");
        var isGuid = Guid.TryParse(id, out var result);
        if (isGuid)
        {
            return result;
        }

        return null;
    }

    /// <summary>
    /// Gets tracking ID of the current user.
    /// </summary>
    /// <returns>The tracking ID if it exists, otherwise empty string.</returns>
    public string GetTrackingId()
    {
        if (!_configuration.GetValue<bool>("Switches:Apm"))
        {
            return _httpContextAccessor.HttpContext?.TraceIdentifier ?? string.Empty;
        }

        var t = Elastic.Apm.Agent.Tracer.CurrentTransaction;
        if (t is not null)
        {
            return t.TraceId;
        }
        return _httpContextAccessor.HttpContext?.TraceIdentifier ?? string.Empty;
    }
}
