// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Constants;
using Microsoft.OpenApi.Models;

namespace MaaCopilotServer.Api.Swagger;

/// <summary>
///     Configures Swagger.
/// </summary>
public static class ConfigureSwagger
{
    /// <summary>
    ///     Add swagger services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddMaaSwagger(this IServiceCollection services)
    {
        if (GlobalConstants.IsProductionEnvironment)
        {
            return services;
        }

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("maa-copilot-document", new OpenApiInfo
            {
                Version = "v1",
                Title = "Maa Copilot Server",
                Description = "Maa Copilot Server API Documentation",
                Contact = new OpenApiContact
                {
                    Name = "Maa Team",
                    Url = new Uri("https://www.maa.plus/")
                },
                License = new OpenApiLicense
                {
                    Name = "AGPL-3.0",
                    Url = new Uri("https://github.com/MaaAssistantArknights/MaaCopilotServer/blob/main/LICENSE")
                },
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });

            options.OperationFilter<AuthorizedFilter>();

            var apiDoc = Path.Combine(AppContext.BaseDirectory, "MaaCopilotServer.Api.xml");
            var applicationDoc = Path.Combine(AppContext.BaseDirectory, "MaaCopilotServer.Application.xml");
            options.IncludeXmlComments(apiDoc);
            options.IncludeXmlComments(applicationDoc);
        });

        return services;
    }

    /// <summary>
    ///     Add swagger middleware.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns><see cref="IApplicationBuilder"/></returns>
    public static IApplicationBuilder UseMaaSwagger(this IApplicationBuilder app)
    {
        if (GlobalConstants.IsProductionEnvironment)
        {
            return app;
        }

        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/openapi.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/maa-copilot-document/openapi.json", "Maa Copilot Server");
        });

        return app;
    }
}
