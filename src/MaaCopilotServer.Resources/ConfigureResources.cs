// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.Extensions.DependencyInjection;

namespace MaaCopilotServer.Resources;

public static class ConfigureResources
{
    public static IServiceCollection AddResources(this IServiceCollection services)
    {
        services.AddScoped<ValidationErrorMessage>();
        services.AddScoped<ApiErrorMessage>();

        return services;
    }
}
