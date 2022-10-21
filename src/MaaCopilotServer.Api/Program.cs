// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Api.Formatter;
using MaaCopilotServer.Api.Helper;
using MaaCopilotServer.Api.Middleware;
using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application;
using MaaCopilotServer.Infrastructure;
using MaaCopilotServer.Resources;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Debugging;

namespace MaaCopilotServer.Api;

/// <summary>
///     The program entry point.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Program
{
    /// <summary>
    ///     The entry point.
    /// </summary>
    public static void Main()
    {
        // Get global configuration.
        var configuration = ConfigurationHelper.BuildConfiguration();

        // Create logger.
        Log.Logger = configuration.GetLoggerConfiguration().CreateLogger();
        SelfLog.Enable(Console.Error); // Direct log output to standard error stream.

        var builder = WebApplication.CreateBuilder();

        builder.Configuration.AddConfiguration(configuration);

        builder.Host.UseSerilog();

        builder.Services.AddCors();
        builder.Services.AddControllers(options =>
            options.OutputFormatters.Insert(0, new MaaResponseFormatter()));
        builder.Services.AddResources();
        builder.Services.AddInfrastructureServices(configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddApiServices(configuration);
        builder.Services.AddMaaSwagger();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
        });

        var app = builder.Build();

        app.UseForwardedHeaders();
        
        app.UseMaaSwagger();

        // CORS settings.
        app.UseCors(options =>
        {
            options.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });

        app.UseRequestCulture();
        app.UseSystemStatus();
        app.UseAuthentication();

        var fileMimeMapping = new Dictionary<string, string> { { ".json", "application/json" } };
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = new FileExtensionContentTypeProvider(fileMimeMapping),
            FileProvider = new PhysicalFileProvider(GlobalConstants.StaticDirectory),
            HttpsCompression = HttpsCompressionMode.Compress,
            RequestPath = "/static",
            ServeUnknownFileTypes = false
        });

        app.MapControllers();

        // Start application.
        app.Run();
    }
}
