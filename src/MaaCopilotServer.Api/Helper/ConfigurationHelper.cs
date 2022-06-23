// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
///     The helper class of the configurations of the application.
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfigurationHelper
{
    /// <summary>
    /// The global setting helper.
    /// </summary>
    private readonly GlobalSettingsHelper _settings;

    /// <summary>
    /// The constructor.
    /// </summary>
    public ConfigurationHelper() : this(new GlobalSettingsHelper())
    { }

    /// <summary>
    /// The constructor with all properties.
    /// </summary>
    /// <param name="globalSettingHelper"></param>
    public ConfigurationHelper(GlobalSettingsHelper globalSettingHelper)
    {
        this._settings = globalSettingHelper;
    }

    /// <summary>
    /// Ensures settings files are created correctly.
    /// </summary>
    private void EnsureSettingsFileCreated()
    {
        // Get settings file locations.
        var appsettingsFile = new FileInfo(this._settings.AppSettings);
        var appsettingsEnvFile =
            new FileInfo(this._settings.AppSettingsEnv);
        var originalAppsettingsFile =
            new FileInfo(this._settings.OriginalAppSettings).AssertExist();
        var originalAppsettingsEnvFile =
            new FileInfo(this._settings.OriginalAppSettingsEnv);

        if (appsettingsFile.Exists is false)
        {
            // Settings file does not exist. Create a new one.
            appsettingsFile.EnsureDeleted();

            var text = File.ReadAllText(originalAppsettingsFile.FullName);
            text = text.Replace("{{ DATA DIRECTORY }}", this._settings.DataDirectory);
            File.WriteAllText(appsettingsFile.FullName, text);

            if (this._settings.IsProductionEnvironment)
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
    public IConfiguration BuildConfiguration()
    {
        EnsureSettingsFileCreated();

        // Build configurations.
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.AddJsonFile(this._settings.AppSettings, false, true);
        configurationBuilder.AddJsonFile(this._settings.AppSettingsEnv, true, true);

        configurationBuilder.AddEnvironmentVariables("MAA_");

        var appVersion = this._settings.AppVersion;

        configurationBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
        {
            new("Application:AssemblyPath", this._settings.AssemblyDirectory),
            new("Application:DataDirectory", this._settings.DataDirectory),
            new("Application:Version", appVersion),
            new("ElasticApm:ServiceVersion", appVersion)
        });

        var configuration = configurationBuilder.Build();
        return configuration;
    }
}
