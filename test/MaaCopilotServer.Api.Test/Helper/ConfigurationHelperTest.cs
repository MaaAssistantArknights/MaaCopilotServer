// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Api.Helper;
using MaaCopilotServer.Infrastructure.Adapters;
using Microsoft.Extensions.Configuration;

namespace MaaCopilotServer.Api.Test.Helper;

/// <summary>
/// Tests <see cref="ConfigurationHelper"/>.
/// </summary>
[ExcludeFromCodeCoverage]
[TestClass]
public class ConfigurationHelperTest
{
    /// <summary>
    /// The old value of <see cref="ConfigurationHelper.SystemAdapter"/>.
    /// </summary>
    private ISystemAdapter _oldSystemAdapter = default!;

    /// <summary>
    /// The temp path of OS.
    /// </summary>
    private static readonly string s_tempPath = Path.GetTempPath();

    /// <summary>
    /// The temp MAA directory.
    /// </summary>
    private static readonly string s_maaDir = Path.Combine(s_tempPath, "maa_test");

    /// <summary>
    /// The temp MAA data directory.
    /// </summary>
    private static readonly string s_maaDataDir = Path.Combine(s_maaDir, "data");

    /// <summary>
    /// The temp MAA app settings JSON file.
    /// </summary>
    private static readonly string s_appSettings = Path.Combine(s_maaDataDir, "appsettings.json");

    /// <summary>
    /// Sets environment variable of the development/production environment.
    /// </summary>
    /// <param name="env">The environment.</param>
    private static void SetEnv(string env)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", env);
    }

    /// <summary>
    /// Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _oldSystemAdapter = ConfigurationHelper.SystemAdapter;
        ConfigurationHelper.SystemAdapter = Mock.Of<ISystemAdapter>();

        // Environment variables
        Environment.SetEnvironmentVariable("MAA_DATA_DIRECTORY", s_maaDataDir);

        if (File.Exists(s_appSettings))
        {
            File.Delete(s_appSettings);
        }
        if (File.Exists(GlobalConstants.OriginalAppSettingsEnv))
        {
            File.Delete(GlobalConstants.OriginalAppSettingsEnv);
        }
    }

    /// <summary>
    /// Tests <see cref="ConfigurationHelper.BuildConfiguration"/>.
    /// </summary>
    [TestMethod]
    public void TestBuildConfiguration()
    {
        SetEnv("Development");

        var configuration = ConfigurationHelper.BuildConfiguration();
        configuration.GetRequiredSection("Test").Value.Should().Be("test");
        configuration.GetRequiredSection("TestDataDirectory").Value.Should().Be(s_maaDataDir);
    }

    /// <summary>
    /// Tests <see cref="ConfigurationHelper.BuildConfiguration"/> with environment file exists.
    /// </summary>
    [TestMethod]
    public void TestBuildConfigurationWithEnvFile()
    {
        SetEnv("Development");
        if (File.Exists(GlobalConstants.OriginalAppSettingsEnv))
        {
            File.Delete(GlobalConstants.OriginalAppSettingsEnv);
        }
        File.Copy(GlobalConstants.OriginalAppSettings, GlobalConstants.OriginalAppSettingsEnv);

        var configuration = ConfigurationHelper.BuildConfiguration();
        configuration.GetRequiredSection("Test").Value.Should().Be("test");

        // Since we have copied original settings to overwrite it, it should be unchanged.
        configuration.GetRequiredSection("TestDataDirectory").Value.Should().Be("{{ DATA DIRECTORY }}");
    }

    /// <summary>
    /// Tests <see cref="ConfigurationHelper.BuildConfiguration"/> with app settings file not exist in production environment.
    /// </summary>
    [TestMethod]
    public void TestBuildConfigurationInProductionEnv()
    {
        SetEnv("Production");
        ConfigurationHelper.BuildConfiguration();

        // Nothing to assert.
    }

    /// <summary>
    /// Cleans up tests.
    /// </summary>
    [TestCleanup]
    public void CleanUp()
    {
        ConfigurationHelper.SystemAdapter = _oldSystemAdapter;

        if (File.Exists(s_appSettings))
        {
            File.Delete(s_appSettings);
        }

        if (File.Exists(GlobalConstants.OriginalAppSettingsEnv))
        {
            File.Delete(GlobalConstants.OriginalAppSettingsEnv);
        }
    }
}
