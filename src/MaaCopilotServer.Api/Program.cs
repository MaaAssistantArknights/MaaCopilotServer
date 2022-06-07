// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Elastic.Apm.AspNetCore;
using Elastic.Apm.EntityFrameworkCore;
using Elastic.CommonSchema.Serilog;
using MaaCopilotServer.Api;
using MaaCopilotServer.Api.Helper;
using MaaCopilotServer.Application;
using MaaCopilotServer.Infrastructure;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var configuration = ConfigurationHelper.BuildConfiguration();

var loggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration);

if (configuration.GetValue<bool>("ElasticLogSink:Enabled"))
{
    var elasticUris = configuration.GetValue<string>("ElasticLogSink:Uris").Split(";").Select(x => new Uri(x)).ToArray();
    var elasticPeriod = configuration.GetValue<int>("ElasticLogSink:Period");
    var elasticApiId = configuration.GetValue<string>("ElasticLogSink:ApiId");
    var elasticApiKey = configuration.GetValue<string>("ElasticLogSink:ApiKey");
    loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticUris)
    {
        Period = TimeSpan.FromSeconds(elasticPeriod),
        AutoRegisterTemplate = true,
        IndexFormat = "maa-copilot-{0:yyyy.MM.dd}",
        ModifyConnectionSettings = c =>
            c.ApiKeyAuthentication(elasticApiId, elasticApiKey),
        CustomFormatter = new EcsTextFormatter()
    });
}

Log.Logger = loggerConfiguration.CreateLogger();

// Alpha 测试版本使用
ConfigurationHelper.LogConfigurations(configuration, Log.Logger);

var builder = WebApplication.CreateBuilder();

builder.Host.UseSerilog();

builder.Configuration.AddConfiguration(configuration);

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddApiServices(configuration);

var app = builder.Build();

DatabaseHelper.DatabaseInitialize(configuration);

if (configuration.GetValue<bool>("ElasticApm:Enabled"))
{
    app.UseElasticApm(configuration, new EfCoreDiagnosticsSubscriber());
}

app.UseAuthentication();
app.MapControllers();

app.Run();
