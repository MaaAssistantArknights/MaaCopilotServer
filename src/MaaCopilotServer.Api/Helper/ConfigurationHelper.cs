// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
///     The helper class of the configurations of the application.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ConfigurationHelper
{
    /// <summary>
    /// Ensures settings files are created correctly.
    /// </summary>
    private static void EnsureSettingsFileCreated()
    {
        // Get settings file locations.
        var appsettingsFile = new FileInfo(GlobalConstants.AppSettings);
        var appsettingsEnvFile =
            new FileInfo(GlobalConstants.AppSettingsEnv);
        var originalAppsettingsFile =
            new FileInfo(GlobalConstants.OriginalAppSettings).AssertExist();
        var originalAppsettingsEnvFile =
            new FileInfo(GlobalConstants.OriginalAppSettingsEnv);

        if (appsettingsFile.Exists is false)
        {
            // Settings file does not exist. Create a new one.
            appsettingsFile.EnsureDeleted();

            var text = File.ReadAllText(originalAppsettingsFile.FullName);
            text = text.Replace("{{ DATA DIRECTORY }}", GlobalConstants.DataDirectory);
            File.WriteAllText(appsettingsFile.FullName, text);

            if (GlobalConstants.IsProductionEnvironment)
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
    }

    /// <summary>
    ///     构建 <see cref="IConfiguration" />
    /// </summary>
    /// <remarks>不适用于 Azure Functions 等云服务</remarks>
    /// <returns><see cref="IConfiguration" /> 实例 (<see cref="ConfigurationRoot" /> 对象)</returns>
    public static IConfiguration BuildConfiguration()
    {
        EnsureSettingsFileCreated();

        // Build configurations.
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile(GlobalConstants.AppSettings, false, true);
        configurationBuilder.AddJsonFile(GlobalConstants.AppSettingsEnv, true, true);

        configurationBuilder.AddEnvironmentVariables("MAA_");

        var appVersion = GlobalConstants.AppVersion;

        configurationBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
        {
            new("Application:AssemblyPath", GlobalConstants.AssemblyDirectory),
            new("Application:DataDirectory", GlobalConstants.DataDirectory),
            new("Application:Version", appVersion),
            new("ElasticApm:ServiceVersion", appVersion)
        });

        var configuration = configurationBuilder.Build();
        return configuration;
    }
}
