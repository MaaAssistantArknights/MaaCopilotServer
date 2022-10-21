// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Infrastructure.Adapters;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
///     The helper class of the configurations of the application.
/// </summary>
public static class ConfigurationHelper
{
    #region Test
    /// <summary>
    /// The system adapter.
    /// </summary>
    public static ISystemAdapter SystemAdapter { get; set; } = new SystemAdapter();
    #endregion

    /// <summary>
    ///     Ensures settings files are created correctly.
    /// </summary>
    private static void EnsureSettingsFileCreated()
    {
        // Get settings file locations.
        var appsettingsFile = new FileInfo(GlobalConstants.AppSettings);
        var appsettingsEnvFile = new FileInfo(GlobalConstants.AppSettingsEnv);
        var originalAppsettingsFile = new FileInfo(GlobalConstants.OriginalAppSettings).AssertExist();
        var originalAppsettingsEnvFile = new FileInfo(GlobalConstants.OriginalAppSettingsEnv);

        if (appsettingsFile.Exists is false)
        {
            // Settings file does not exist. Create a new one.
            appsettingsFile.EnsureDeleted();

            var text = File.ReadAllText(originalAppsettingsFile.FullName);
            text = text.Replace("{{ DATA DIRECTORY }}", JsonSerializer.Serialize(GlobalConstants.DataDirectory).Replace("\"", ""));
            File.WriteAllText(appsettingsFile.FullName, text);

            if (GlobalConstants.IsProductionEnvironment)
            {
                // TODO:
                // Shall we throw an exception when the settings file does not exist?
                // We can either catch it or not in the main function.
                // Or shall we exit with non-zero status?
                SystemAdapter.Exit(0);
                return;
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
    ///     Construct <see cref="IConfiguration" />.
    /// </summary>
    /// <returns><see cref="IConfiguration" /> (<see cref="ConfigurationRoot"/>) instance.</returns>
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
            new("Application:Version", appVersion)
        });

        var configuration = configurationBuilder.Build();
        return configuration;
    }
}
