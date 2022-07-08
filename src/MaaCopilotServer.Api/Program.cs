// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using Elastic.Apm.Elasticsearch;
using Elastic.Apm.EntityFrameworkCore;
using Elastic.Apm.Extensions.Hosting;
using MaaCopilotServer.Api.Formatter;
using MaaCopilotServer.Api.Helper;
using MaaCopilotServer.Api.Middleware;
using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure;
using MaaCopilotServer.Resources;
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

        InitializeHelper.InitializeEmailTemplates();

        var builder = WebApplication.CreateBuilder();

        builder.Configuration.AddConfiguration(configuration);

        builder.Host.UseSerilog();

        var switchesOption = configuration.GetOption<SwitchesOption>();
        if (switchesOption.Apm)
        {
            builder.Host.UseElasticApm(
                new EfCoreDiagnosticsSubscriber(),
                new ElasticsearchDiagnosticsSubscriber());
        }

        builder.Services.AddCors();
        builder.Services.AddControllers(options =>
            options.OutputFormatters.Insert(0, new MaaResponseFormatter()));
        builder.Services.AddResources();
        builder.Services.AddInfrastructureServices(configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddApiServices(configuration);
        builder.Services.AddMaaSwagger();

        var app = builder.Build();

        app.UseMaaSwagger();

        // CORS settings.
        app.UseCors(options =>
        {
            options.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });

        app.UseApmTransaction();
        app.UseRequestCulture();
        app.UseSystemStatus();
        app.UseAuthentication();
        app.MapControllers();

        // Start application.
        app.Run();
    }
}
