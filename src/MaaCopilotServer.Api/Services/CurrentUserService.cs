// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using System.Security.Claims;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Api.Services;

/// <summary>
///     The service for parsing information of the current user.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    /// <summary>
    ///     The configuration.
    /// </summary>
    private readonly IConfiguration _configuration;

    private readonly IMaaCopilotDbContext _dbContext;

    /// <summary>
    ///     The HTTP context accessor.
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    ///     The constructor of <see cref="CurrentUserService" />.
    /// </summary>
    /// <param name="dbContext">The db context.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="configuration">The configuration.</param>
    public CurrentUserService(
        IMaaCopilotDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<CopilotUser?> GetUser()
    {
        var identity = this.GetUserIdentity();
        if (identity is null)
        {
            return null;
        }

        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == identity.Value);
        return user;
    }

    /// <inheritdoc />
    public string GetTrackingId()
    {
        return _httpContextAccessor.HttpContext?.TraceIdentifier ?? string.Empty;
    }

    /// <inheritdoc />
    public CultureInfo GetCulture()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is null)
        {
            return new CultureInfo("zh-cn");
        }

        var hasValue = context.Items.TryGetValue("culture", out var cultureInfo);
        if (hasValue is false || cultureInfo is null || cultureInfo.GetType().Name != "CultureInfo")
        {
            return new CultureInfo("zh-cn");
        }

        return (CultureInfo)cultureInfo;
    }
}
