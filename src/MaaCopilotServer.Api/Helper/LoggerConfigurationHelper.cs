// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Destructurama;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
///     The helper class of logger.
/// </summary>
public static class LoggerConfigurationHelper
{
    /// <summary>
    ///     Constructs a <see cref="LoggerConfiguration" /> instance based on the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The <see cref="LoggerConfiguration" /> instance.</returns>
    public static LoggerConfiguration GetLoggerConfiguration(this IConfiguration configuration)
    {
        var switchesOption = configuration.GetOption<SwitchesOption>();
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Destructure.UsingAttributes();

        if (switchesOption.ElasticSearch is false)
        {
            return loggerConfiguration;
        }

        // Elastic search options.
        var elasticOptions = configuration.GetOption<ElasticLogSinkOption>();
        var elasticUris = elasticOptions.Uris.Split(";").Select(x => new Uri(x)).ToArray();
        loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticUris)
        {
            Period = TimeSpan.FromSeconds(elasticOptions.Period),
            AutoRegisterTemplate = true,
            IndexFormat = $"{elasticOptions.ApplicationName}-{DateTimeOffset.UtcNow.AddHours(8):yyyy.MM}",
            CustomFormatter = new EcsTextFormatter(),
            TypeName = null,
            ModifyConnectionSettings = c =>
            {
                switch (elasticOptions.Authentication.Method)
                {
                    case "Basic":
                        c.BasicAuthentication(elasticOptions.Authentication.Secret.Id,
                            elasticOptions.Authentication.Secret.Key);
                        break;
                    case "ApiKey":
                        c.ApiKeyAuthentication(elasticOptions.Authentication.Secret.Id,
                            elasticOptions.Authentication.Secret.Key);
                        break;
                }

                return c;
            }
        });
        loggerConfiguration.Enrich.WithElasticApmCorrelationInfo();

        return loggerConfiguration;
    }
}
