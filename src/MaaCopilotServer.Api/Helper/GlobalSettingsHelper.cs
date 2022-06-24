// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
/// The helper of global settings.
/// </summary>
[ExcludeFromCodeCoverage]
public class GlobalSettingsHelper
{
    /// <summary>
    /// The production environment string.
    /// </summary>
    public static readonly string ProductionEnvironment = "Production";

    /// <summary>
    /// The <c>MAA_DATA_DIRECTORY</c> environment variable.
    /// </summary>
    private string MaaDataDirectory => Environment.GetEnvironmentVariable("MAA_DATA_DIRECTORY") ?? "";

    /// <summary>
    /// The <c>DOTNET_RUNNING_IN_CONTAINER</c> environment variable.
    /// </summary>
    public string IsDotnetRunningInContainer =>
        Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false";

    /// <summary>
    /// The executing assembly directory.
    /// </summary>
    public string AssemblyDirectory => new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;

    /// <summary>
    /// The data directory.
    /// </summary>
    public string DataDirectory =>
        string.IsNullOrEmpty(MaaDataDirectory)
            ? new DirectoryInfo(AssemblyDirectory.CombinePath("data")).EnsureCreated().FullName
            : new DirectoryInfo(MaaDataDirectory).EnsureCreated().FullName;

    /// <summary>
    /// The ASP.NET Core environment.
    /// </summary>
    private string AspNetCoreEnvironment =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    /// <summary>
    /// Indicates whether the current environemnt is production.
    /// </summary>
    public bool IsProductionEnvironment => AspNetCoreEnvironment == ProductionEnvironment;

    /// <summary>
    /// The <c>appsettings.json</c> file location.
    /// </summary>
    public string AppSettings => DataDirectory.CombinePath("appsettings.json");

    /// <summary>
    /// The <c>appsettings.&lt;env&gt;.json</c> file location.
    /// </summary>
    public string AppSettingsEnv => DataDirectory.CombinePath($"appsettings.{AspNetCoreEnvironment}.json");

    /// <summary>
    /// The original <c>appsettings.json</c> file location.
    /// </summary>
    public string OriginalAppSettings => AssemblyDirectory.CombinePath("appsettings.json");

    /// <summary>
    /// The original <c>appsettings.&lt;env&gt;.json</c> file location.
    /// </summary>
    public string OriginalAppSettingsEnv => AssemblyDirectory.CombinePath($"appsettings.{AspNetCoreEnvironment}.json");

    /// <summary>
    /// The App version.
    /// </summary>
    public string AppVersion => Environment.GetEnvironmentVariable("MAACOPILOT_APP_VERSION") ?? "0.0.0";

    /// <summary>
    /// The original templates directory.
    /// </summary>
    public string OriginalTemplatesDirectory => AssemblyDirectory.CombinePath("templates");

    /// <summary>
    /// The target templates directory.
    /// </summary>
    public string TargetTemplatesDirectory => DataDirectory.CombinePath("templates");

    /// <summary>
    /// The default user email.
    /// </summary>
    public string DefaultUserEmail => Environment.GetEnvironmentVariable("DEFAULT_USER_EMAIL") ?? "super@prts.plus";

    /// <summary>
    /// Indicates whether the default user email is empty.
    /// </summary>
    public bool IsDefaultUserEmailEmpty =>
        string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_USER_EMAIL"));

    /// <summary>
    /// The default user password.
    /// </summary>
    public string DefaultUserPassword => Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD") ?? "";

    /// <summary>
    /// Indicates whether the default user password is empty.
    /// </summary>
    public bool IsDefaultUserPasswordEmpty =>
        string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD"));

    /// <summary>
    /// The default username.
    /// </summary>
    public string DefaultUsername => Environment.GetEnvironmentVariable("DEFAULT_USER_NAME") ?? "Maa";
}
