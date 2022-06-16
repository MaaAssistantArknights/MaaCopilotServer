// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Extensions;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
/// The helper class of the configurations of the application.
/// </summary>
public static class ConfigurationHelper
{
    /// <summary>
    /// 构建 <see cref="IConfiguration"/>
    /// </summary>
    /// <remarks>不适用于 Azure Functions 等云服务</remarks>
    /// <returns><see cref="IConfiguration"/> 实例 (<see cref="ConfigurationRoot"/> 对象)</returns>
    public static IConfiguration BuildConfiguration()
    {
        // Get data directory from environment variables.
        var dataDirectoryEnv = Environment.GetEnvironmentVariable("MAA_DATA_DIRECTORY");
        var isInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false";

        // Get assembly and data directories.
        var assemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.IsNotNull();
        var dataDirectory = string.IsNullOrEmpty(dataDirectoryEnv)
            ? new DirectoryInfo(assemblyDirectory.FullName.CombinePath("data")).EnsureCreated()
            : new DirectoryInfo(dataDirectoryEnv).EnsureCreated();

        // Get DEV/PROD environment.
        var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        // Get settings file locations.
        var appsettingsFile = new FileInfo(dataDirectory.FullName.CombinePath("appsettings.json"));
        var appsettingsEnvFile = new FileInfo(dataDirectory.FullName.CombinePath($"appsettings.{currentEnvironment}.json"));
        var originalAppsettingsFile = new FileInfo(assemblyDirectory.FullName.CombinePath("appsettings.json")).AssertExist();
        var originalAppsettingsEnvFile = new FileInfo(assemblyDirectory.FullName.CombinePath($"appsettings.{currentEnvironment}.json"));

        if (appsettingsFile.Exists is false)
        {
            // Settings file does not exist. Create a new one.
            appsettingsFile.EnsureDeleted();

            var text = File.ReadAllText(originalAppsettingsFile.FullName);
            text = text.Replace("{{ DATA DIRECTORY }}", dataDirectory.FullName);
            File.WriteAllText(appsettingsFile.FullName, text);

            if (currentEnvironment == "Production")
            {
                Environment.Exit(0);
            }
        }

        // Check settings file per environment.
        if (originalAppsettingsEnvFile.Exists)
        {
            originalAppsettingsEnvFile.CopyTo(appsettingsEnvFile.FullName, true);
        }
        else
        {
            appsettingsEnvFile.EnsureDeleted();
        }

        // Build configurations.
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

    /// <summary>
    /// 获取配置项实例
    /// </summary>
    /// <param name="configuration">IConfiguration 实现</param>
    /// <typeparam name="T">Options 类</typeparam>
    /// <returns><typeparamref name="T"/> 的实例</returns>
    /// <exception cref="ArgumentNullException"><typeparamref name="T"/> 无 <see cref="OptionNameAttribute"/> 修饰</exception>
    public static T GetOption<T>(this IConfiguration configuration) where T : class, new()
    {
        var option = new T();
        var attr = typeof(T).ReadAttribute<OptionNameAttribute>();
        if (attr is null)
        {
            throw new ArgumentNullException(typeof(T).FullName);
        }

        configuration.GetSection(attr.OptionName).Bind(option);
        return option;
    }
}
