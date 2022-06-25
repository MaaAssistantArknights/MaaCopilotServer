// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using MaaCopilotServer.Api.Services;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Extensions;
using MaaCopilotServer.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MaaCopilotServer.Api;

/// <summary>
///     The extension to add API service to the services.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    /// <summary>
    ///     Adds API service to the services.
    /// </summary>
    /// <param name="services">The collection of services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The collection of services with the configuration added.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOption = configuration.GetOption<JwtOption>();

        services
            .AddOption<JwtOption>()
            .AddOption<DatabaseOption>()
            .AddOption<ElasticLogSinkOption>()
            .AddOption<SwitchesOption>()
            .AddOption<EmailOption>()
            .AddOption<ApplicationOption>()
            .AddOption<TokenOption>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.Secret))
                };
            });

        return services;
    }

    private static IServiceCollection AddOption<T>(this IServiceCollection services)
        where T : class, new()
    {
        services.AddOptions<T>().BindConfiguration(typeof(T).ReadAttribute<OptionNameAttribute>()!.OptionName);
        return services;
    }
}
