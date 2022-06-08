// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Destructurama;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace MaaCopilotServer.Api.Helper;

public static class LoggerConfigurationHelper
{
    public static LoggerConfiguration GetLoggerConfiguration(this IConfiguration configuration)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Destructure.UsingAttributes();

        if (configuration.GetValue<bool>("Switches:ElasticSearch") is false)
        {
            return loggerConfiguration;
        }

        var elasticUris = configuration.GetValue<string>("ElasticLogSink:Uris")
            .Split(";").Select(x => new Uri(x)).ToArray();
        var elasticPeriod = configuration.GetValue<int>("ElasticLogSink:Period");
        var elasticAuthMethod = configuration.GetValue<string>("ElasticLogSink:Authentication:Method");
        var elasticId = configuration.GetValue<string>("ElasticLogSink:Authentication:Secret:Id");
        var elasticKey = configuration.GetValue<string>("ElasticLogSink:Authentication:Secret:Key");
        var elasticApplicationName = configuration.GetValue<string>("ElasticLogSink:ApplicationName");
        loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticUris)
        {
            Period = TimeSpan.FromSeconds(elasticPeriod),
            AutoRegisterTemplate = true,
            IndexFormat = $"{elasticApplicationName}-{DateTimeOffset.UtcNow.AddHours(8):yyyy.MM}",
            CustomFormatter = new EcsTextFormatter(),
            TypeName = null,
            ModifyConnectionSettings = c =>
            {
                switch (elasticAuthMethod)
                {
                    case "Basic":
                        c.BasicAuthentication(elasticId, elasticKey);
                        break;
                    case "ApiKey":
                        c.ApiKeyAuthentication(elasticId, elasticKey);
                        break;
                }
                return c;
            }
        });
        loggerConfiguration.Enrich.WithElasticApmCorrelationInfo();

        return loggerConfiguration;
    }
}
