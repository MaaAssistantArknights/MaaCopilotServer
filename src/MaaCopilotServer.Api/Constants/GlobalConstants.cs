// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Api.Constants;

/// <summary>
/// The helper of global settings.
/// </summary>
[ExcludeFromCodeCoverage]
public static class GlobalConstants
{
    /// <summary>
    /// The production environment string.
    /// </summary>
    public static readonly string ProductionEnvironment = "Production";

    /// <summary>
    /// The <c>MAA_DATA_DIRECTORY</c> environment variable.
    /// </summary>
    private static string MaaDataDirectory => Environment.GetEnvironmentVariable("MAA_DATA_DIRECTORY") ?? "";

    /// <summary>
    /// The <c>DOTNET_RUNNING_IN_CONTAINER</c> environment variable.
    /// </summary>
    public static string IsDotnetRunningInContainer =>
        Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false";

    /// <summary>
    /// The executing assembly directory.
    /// </summary>
    public static string AssemblyDirectory => new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;

    /// <summary>
    /// The data directory.
    /// </summary>
    public static string DataDirectory =>
        string.IsNullOrEmpty(MaaDataDirectory)
            ? new DirectoryInfo(AssemblyDirectory.CombinePath("data")).EnsureCreated().FullName
            : new DirectoryInfo(MaaDataDirectory).EnsureCreated().FullName;

    /// <summary>
    /// The static file directory.
    /// </summary>
    public static string StaticDirectory => new DirectoryInfo(AssemblyDirectory.CombinePath("static")).FullName;

    /// <summary>
    /// The ASP.NET Core environment.
    /// </summary>
    private static string AspNetCoreEnvironment =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    /// <summary>
    /// Indicates whether the current environment is production.
    /// </summary>
    public static bool IsProductionEnvironment => AspNetCoreEnvironment == ProductionEnvironment;

    /// <summary>
    /// The <c>appsettings.json</c> file location.
    /// </summary>
    public static string AppSettings => DataDirectory.CombinePath("appsettings.json");

    /// <summary>
    /// The <c>appsettings.&lt;env&gt;.json</c> file location.
    /// </summary>
    public static string AppSettingsEnv => DataDirectory.CombinePath($"appsettings.{AspNetCoreEnvironment}.json");

    /// <summary>
    /// The original <c>appsettings.json</c> file location.
    /// </summary>
    public static string OriginalAppSettings => AssemblyDirectory.CombinePath("appsettings.json");

    /// <summary>
    /// The original <c>appsettings.&lt;env&gt;.json</c> file location.
    /// </summary>
    public static string OriginalAppSettingsEnv => AssemblyDirectory.CombinePath($"appsettings.{AspNetCoreEnvironment}.json");

    /// <summary>
    /// The App version.
    /// </summary>
    public static string AppVersion => Environment.GetEnvironmentVariable("MAA_COPILOT_APP_VERSION") ?? "0.0.0";

    /// <summary>
    /// The original templates directory.
    /// </summary>
    public static string OriginalTemplatesDirectory => AssemblyDirectory.CombinePath("templates");

    /// <summary>
    /// The target templates directory.
    /// </summary>
    public static string TargetTemplatesDirectory => DataDirectory.CombinePath("templates");

    /// <summary>
    /// The default user email.
    /// </summary>
    public static string DefaultUserEmail => Environment.GetEnvironmentVariable("MAA_DEFAULT_USER_EMAIL") ?? "super@prts.plus";

    /// <summary>
    /// Indicates whether the default user email is empty.
    /// </summary>
    public static bool IsDefaultUserEmailEmpty =>
        string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MAA_DEFAULT_USER_EMAIL"));

    /// <summary>
    /// The default user password.
    /// </summary>
    public static string DefaultUserPassword => Environment.GetEnvironmentVariable("MAA_DEFAULT_USER_PASSWORD") ?? "";

    /// <summary>
    /// Indicates whether the default user password is empty.
    /// </summary>
    public static bool IsDefaultUserPasswordEmpty =>
        string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MAA_DEFAULT_USER_PASSWORD"));

    /// <summary>
    /// The default username.
    /// </summary>
    public static string DefaultUsername => Environment.GetEnvironmentVariable("MAA_DEFAULT_USER_NAME") ?? "Maa";
}
