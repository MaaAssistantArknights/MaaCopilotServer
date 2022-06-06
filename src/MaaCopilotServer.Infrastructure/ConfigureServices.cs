// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Infrastructure.Database;
using MaaCopilotServer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MaaCopilotServer.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<IMaaCopilotDbContext, MaaCopilotDbContext>();

        services.AddScoped<IIdentityService, IdentityService>();

        services.AddSingleton<ICopilotIdService, CopilotIdService>();

        return services;
    }
}
