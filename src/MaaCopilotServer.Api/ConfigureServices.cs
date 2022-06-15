// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Net;
using System.Text;
using MaaCopilotServer.Api.Helper;
using MaaCopilotServer.Api.Services;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Extensions;
using MaaCopilotServer.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MaaCopilotServer.Api;

/// <summary>
/// The service of configuration.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds API service to the services.
    /// </summary>
    /// <param name="services">The collection of services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The collection of services with the configuration added.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOption = configuration.GetOption<JwtOption>();

        services
            .AddOption<JwtOption>(configuration)
            .AddOption<DatabaseOption>(configuration)
            .AddOption<ElasticLogSinkOption>(configuration)
            .AddOption<SwitchesOption>(configuration);

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

                options.Events = new JwtBearerEvents
                {
                    // Set token in the context.
                    OnMessageReceived = (context) =>
                    {
                        if (!context.Request.Query.TryGetValue("access_token", out var values))
                        {
                            return Task.CompletedTask;
                        }

                        if (values.Count > 1)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Fail(
                                "Only one 'access_token' query string parameter can be defined. " +
                                $"However, {values.Count:N0} were included in the request."
                            );
                            return Task.CompletedTask;
                        }

                        var token = values.Single();
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Fail(
                                "The 'access_token' query string parameter was defined, " +
                                "but a value to represent the token was not included."
                            );

                            return Task.CompletedTask;
                        }

                        context.Token = token;
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    private static IServiceCollection AddOption<T>(this IServiceCollection services, IConfiguration configuration) where T : class, new()
    {
        services.AddOptions<T>().BindConfiguration(typeof(T).ReadAttribute<OptionNameAttribute>()!.OptionName);
        return services;
    }
}
