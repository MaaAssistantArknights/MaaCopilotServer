// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api;
using MaaCopilotServer.Api.Helper;
using MaaCopilotServer.Application;
using MaaCopilotServer.Infrastructure;
using Serilog;

var configuration = ConfigurationHelper.BuildConfiguration();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = WebApplication.CreateBuilder();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddApiServices();

var app = builder.Build();

app.MapControllers();

app.Run();
