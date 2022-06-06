// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Security.Claims;
using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

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

    public string GetTrackingId()
    {
        return _httpContextAccessor.HttpContext?.TraceIdentifier ?? string.Empty;
    }
}
