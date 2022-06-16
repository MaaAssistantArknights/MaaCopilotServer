// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Extensions;

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

        var assemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.IsNotNull();
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

            if (currentEnvironment == "Production")
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

        var appVersion = Environment.GetEnvironmentVariable("MAACOPILOT_APP_VERSION") ?? "0.0.0";

        configurationBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
        {
            new("Application:AssemblyPath", assemblyDirectory.FullName),
            new("Application:DataDirectory", dataDirectory.FullName),
            new("Application:Version", appVersion),
            new("ElasticApm:ServiceVersion", appVersion)
        });

        var configuration = configurationBuilder.Build();
        return configuration;
    }
}
