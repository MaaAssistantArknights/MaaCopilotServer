// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Security.Claims;
using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
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
        if (_configuration.GetValue<bool>("ElasticApm:Enabled") is false)
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
