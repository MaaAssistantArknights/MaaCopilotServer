// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Elastic.Apm.AspNetCore;
using Elastic.Apm.AspNetCore.DiagnosticListener;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.Elasticsearch;
using Elastic.Apm.EntityFrameworkCore;
using MaaCopilotServer.Api;
using MaaCopilotServer.Api.Helper;
using MaaCopilotServer.Api.Middleware;
using MaaCopilotServer.Application;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure;
using MaaCopilotServer.Resources;
using Serilog;
using Serilog.Debugging;

// Get global configuration.
var configuration = ConfigurationHelper.BuildConfiguration();

// Create logger.
Log.Logger = configuration.GetLoggerConfiguration().CreateLogger();
SelfLog.Enable(Console.Error); // Direct log output to standard error stream.

InitializeHelper.InitializeEmailTemplates(configuration);

var builder = WebApplication.CreateBuilder();

builder.Host.UseSerilog();

builder.Configuration.AddConfiguration(configuration);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddResources();
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApiServices(configuration);

var app = builder.Build();

InitializeHelper.InitializeDatabase(configuration);

var switchesOption = configuration.GetOption<SwitchesOption>();
if (switchesOption.Apm)
{
    app.UseElasticApm(configuration,
        new EfCoreDiagnosticsSubscriber(),
        new ElasticsearchDiagnosticsSubscriber(),
        new HttpDiagnosticsSubscriber(),
        new AspNetCoreDiagnosticSubscriber(),
        new AspNetCoreErrorDiagnosticsSubscriber());
}

// CORS settings.
app.UseCors(options =>
{
    options.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseRequestCulture();
app.UseAuthentication();
app.MapControllers();

// Start application.
app.Run();
