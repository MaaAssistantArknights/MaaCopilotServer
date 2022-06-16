// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Infrastructure.Database;
using MaaCopilotServer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MaaCopilotServer.Infrastructure;

/// <summary>
/// The extension to add infrastructure services.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds infrastructure services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with the services added.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<IMaaCopilotDbContext, MaaCopilotDbContext>();

        services.AddScoped<IIdentityService, IdentityService>();

        services.AddSingleton<ICopilotIdService, CopilotIdService>();
        services.AddSingleton<ISecretService, SecretService>();

        return services;
    }
}
