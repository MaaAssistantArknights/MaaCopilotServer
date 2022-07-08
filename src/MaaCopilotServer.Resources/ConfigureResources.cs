// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MaaCopilotServer.Resources;

/// <summary>
///     The extension to add resources.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigureResources
{
    /// <summary>
    ///     Adds resources.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with the services added.</returns>
    public static IServiceCollection AddResources(this IServiceCollection services)
    {
        services.AddScoped<ValidationErrorMessage>();
        services.AddScoped<ApiErrorMessage>();
        services.AddScoped<DomainString>();

        return services;
    }
}
