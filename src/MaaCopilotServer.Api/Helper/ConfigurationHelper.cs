// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;
using MaaCopilotServer.Application.Common.Extensions;
using ILogger = Serilog.ILogger;

namespace MaaCopilotServer.Api.Helper;

public static class ConfigurationHelper
{
    /// <summary>
    /// 构建 <see cref="IConfiguration"/>
    /// </summary>
    /// <remarks>不适用于 Azure Functions 等云服务</remarks>
    /// <returns><see cref="IConfiguration"/> 实例 (<see cref="ConfigurationRoot"/> 对象)</returns>
    public static IConfiguration BuildConfiguration()
    {
        var dataDirectoryEnv = Environment.GetEnvironmentVariable("MAA_DATA_DIRECTORY");
        var isInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false";

        var assemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.NotNull();
        var dataDirectory = string.IsNullOrEmpty(dataDirectoryEnv)
            ? new DirectoryInfo(assemblyDirectory.FullName.CombinePath("data")).EnsureCreated()
            : new DirectoryInfo(dataDirectoryEnv).EnsureCreated();

        var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var appsettingsFile = new FileInfo(dataDirectory.FullName.CombinePath("appsettings.json"));
        var appsettingsEnvFile = new FileInfo(dataDirectory.FullName.CombinePath($"appsettings.{currentEnvironment}.json"));
        var originalAppsettingsFile = new FileInfo(assemblyDirectory.FullName.CombinePath("appsettings.json")).AssertExist();
        var originalAppsettingsEnvFile = new FileInfo(assemblyDirectory.FullName.CombinePath($"appsettings.{currentEnvironment}.json"));

        if (appsettingsFile.Exists is false)
        {
            appsettingsFile.EnsureDeleted();

            var text = File.ReadAllText(originalAppsettingsFile.FullName);
            text = text.Replace("{{ DATA DIRECTORY }}", dataDirectory.FullName);
            File.WriteAllText(appsettingsFile.FullName, text);

            if (currentEnvironment == "Production" && isInDocker == "false")
            {
                Environment.Exit(0);
            }
        }
        if (originalAppsettingsEnvFile.Exists)
        {
            originalAppsettingsEnvFile.CopyTo(appsettingsEnvFile.FullName, true);
        }
        else
        {
            appsettingsEnvFile.EnsureDeleted();
        }

        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile(appsettingsFile.FullName, optional: false, reloadOnChange: true);
        configurationBuilder.AddJsonFile(appsettingsEnvFile.FullName, optional: true, reloadOnChange: true);

        configurationBuilder.AddEnvironmentVariables("MAA_");

        configurationBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
        {
            new("Application:AssemblyPath", assemblyDirectory.FullName),
            new("Application:DataDirectory", dataDirectory.FullName)
        });

        var configuration = configurationBuilder.Build();
        return configuration;
    }

    /// <summary>
    /// 开发测试版本使用，便于测试
    /// </summary>
    public static void LogConfigurations(IConfiguration configuration, ILogger logger)
    {
        logger.Information("Configuration Jwt:Token: {@Configuration}", configuration.GetValue<string>("Jwt:Token"));
        logger.Information("Configuration Jwt:Issuer: {@Configuration}", configuration.GetValue<string>("Jwt:Issuer"));
        logger.Information("Configuration Jwt:Audience: {@Configuration}", configuration.GetValue<string>("Jwt:Audience"));
        logger.Information("Configuration Jwt:ExpireTime: {@Configuration}", configuration.GetValue<int>("Jwt:ExpireTime"));
        logger.Information("Configuration Database:ConnectionString: {@Configuration}", configuration.GetValue<string>("Database:ConnectionString"));
        logger.Information("Configuration ElasticLogSink:Enabled: {@Configuration}", configuration.GetValue<bool>("ElasticLogSink:Enabled"));
        logger.Information("Configuration ElasticLogSink:Uris: {@Configuration}", configuration.GetValue<string>("ElasticLogSink:Uris"));
        logger.Information("Configuration ElasticLogSink:Period: {@Configuration}", configuration.GetValue<string>("ElasticLogSink:Period"));
        logger.Information("Configuration ElasticLogSink:ApiId: {@Configuration}", configuration.GetValue<string>("ElasticLogSink:ApiId"));
        logger.Information("Configuration ElasticLogSink:ApiKey: {@Configuration}", configuration.GetValue<string>("ElasticLogSink:ApiKey"));
        logger.Information("Configuration ElasticApm:Enabled: {@Configuration}", configuration.GetValue<bool>("ElasticApm:Enabled"));
        logger.Information("Configuration ElasticApm:SecretToken: {@Configuration}", configuration.GetValue<string>("ElasticApm:SecretToken"));
        logger.Information("Configuration ElasticApm:ServerUrl: {@Configuration}", configuration.GetValue<string>("ElasticApm:ServerUrl"));
        logger.Information("Configuration ElasticApm:ServiceName: {@Configuration}", configuration.GetValue<string>("ElasticApm:ServiceName"));
        logger.Information("Configuration ElasticApm:Environment: {@Configuration}", configuration.GetValue<string>("ElasticApm:Environment"));
    }
}
