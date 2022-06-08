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
using MaaCopilotServer.Application;
using MaaCopilotServer.Infrastructure;
using Serilog;
using Serilog.Debugging;

var configuration = ConfigurationHelper.BuildConfiguration();

Log.Logger = configuration.GetLoggerConfiguration().CreateLogger();
SelfLog.Enable(Console.Error);

var builder = WebApplication.CreateBuilder();

builder.Host.UseSerilog();

builder.Configuration.AddConfiguration(configuration);

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddApiServices(configuration);

var app = builder.Build();

DatabaseHelper.DatabaseInitialize(configuration);

if (configuration.GetValue<bool>("Switches:Apm"))
{
    app.UseElasticApm(configuration,
        new EfCoreDiagnosticsSubscriber(),
        new ElasticsearchDiagnosticsSubscriber(),
        new HttpDiagnosticsSubscriber(),
        new AspNetCoreDiagnosticSubscriber(),
        new AspNetCoreErrorDiagnosticsSubscriber());
}

app.UseAuthentication();
app.MapControllers();

app.Run();
