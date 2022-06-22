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
    private string MaaDataDirectory
    {
        get
        {
            return Environment.GetEnvironmentVariable("MAA_DATA_DIRECTORY") ?? "";
        }
    }

    /// <summary>
    /// The <c>DOTNET_RUNNING_IN_CONTAINER</c> environment variable.
    /// </summary>
    public string IsDotnetRunningInContainer
    {
        get
        {
            return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false";
        }
    }

    /// <summary>
    /// The executing assembly directory.
    /// </summary>
    public string AssemblyDirectory
    {
        get
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;
        }
    }

    /// <summary>
    /// The data directory.
    /// </summary>
    public string DataDirectory
    {
        get
        {
            return string.IsNullOrEmpty(this.MaaDataDirectory)
            ? new DirectoryInfo(this.AssemblyDirectory.CombinePath("data")).EnsureCreated().FullName
            : new DirectoryInfo(this.MaaDataDirectory).EnsureCreated().FullName;
        }
    }

    /// <summary>
    /// The ASP.NET Core environment.
    /// </summary>
    private string AspNetCoreEnvironment
    {
        get
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        }
    }

    /// <summary>
    /// Indicates whether the current environemnt is production.
    /// </summary>
    public bool IsProductionEnvironment
    {
        get
        {
            return this.AspNetCoreEnvironment == ProductionEnvironment;
        }
    }

    /// <summary>
    /// The <c>appsettings.json</c> file location.
    /// </summary>
    public string AppSettings
    {
        get
        {
            return this.DataDirectory.CombinePath("appsettings.json");
        }
    }

    /// <summary>
    /// The <c>appsettings.&lt;env&gt;.json</c> file location.
    /// </summary>
    public string AppSettingsEnv
    {
        get
        {
            return this.DataDirectory.CombinePath($"appsettings.{this.AspNetCoreEnvironment}.json");
        }
    }

    /// <summary>
    /// The original <c>appsettings.json</c> file location.
    /// </summary>
    public string OriginalAppSettings
    {
        get
        {
            return this.AssemblyDirectory.CombinePath("appsettings.json");
        }
    }

    /// <summary>
    /// The original <c>appsettings.&lt;env&gt;.json</c> file location.
    /// </summary>
    public string OriginalAppSettingsEnv
    {
        get
        {
            return this.AssemblyDirectory.CombinePath($"appsettings.{this.AspNetCoreEnvironment}.json");
        }
    }

    /// <summary>
    /// The App version.
    /// </summary>
    public string AppVersion
    {
        get
        {
            return Environment.GetEnvironmentVariable("MAACOPILOT_APP_VERSION") ?? "0.0.0";
        }
    }

    /// <summary>
    /// The original templates directory.
    /// </summary>
    public string OriginalTemplatesDirectory
    {
        get
        {
            return this.AssemblyDirectory.CombinePath("templates");
        }
    }

    /// <summary>
    /// The target templates directory.
    /// </summary>
    public string TargetTemplatesDirectory
    {
        get
        {
            return this.DataDirectory.CombinePath("templates");
        }
    }

    /// <summary>
    /// The default user email.
    /// </summary>
    public string DefaultUserEmail
    {
        get
        {
            return Environment.GetEnvironmentVariable("DEFAULT_USER_EMAIL") ?? "super@prts.plus";
        }
    }

    /// <summary>
    /// Indicates whether the default user email is empty.
    /// </summary>
    public bool IsDefaultUserEmailEmpty
    {
        get
        {
            return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_USER_EMAIL"));
        }
    }

    /// <summary>
    /// The default user password.
    /// </summary>
    public string DefaultUserPassword
    {
        get
        {
            return Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD") ?? "";
        }
    }

    /// <summary>
    /// Indicates whether the default user password is empty.
    /// </summary>
    public bool IsDefaultUserPasswordEmpty
    {
        get
        {
            return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD"));
        }
    }

    /// <summary>
    /// The default username.
    /// </summary>
    public string DefaultUsername
    {
        get
        {
            return Environment.GetEnvironmentVariable("DEFAULT_USER_NAME") ?? "Maa";
        }
    }
}
