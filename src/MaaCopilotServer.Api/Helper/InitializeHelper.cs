// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
///     The helper class of database connection.
/// </summary>
public class InitializeHelper
{
    /// <summary>
    /// The configuration.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// The global settings helper.
    /// </summary>
    private readonly GlobalSettingsHelper _settings;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public InitializeHelper(IConfiguration configuration) : this(configuration, new GlobalSettingsHelper())
    {
        _configuration = configuration;
    }

    /// <summary>
    /// The constructor with all properties.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="settings">The global settings helper.</param>
    public InitializeHelper(IConfiguration configuration, GlobalSettingsHelper settings)
    {
        _configuration = configuration;
        _settings = settings;
    }

    /// <summary>
    /// Initializes email templates.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void InitializeEmailTemplates()
    {
        var originalTemplatesDirectory = new DirectoryInfo(_settings.OriginalTemplatesDirectory);
        var targetTemplatesDirectory = new DirectoryInfo(_settings.TargetTemplatesDirectory);

        if (targetTemplatesDirectory.Exists is false)
        {
            targetTemplatesDirectory.Create();
        }

        var originalTemplates = originalTemplatesDirectory.GetFiles();
        var targetTemplates = targetTemplatesDirectory.GetFiles();

        foreach (var originalTemplate in originalTemplates)
        {
            if (targetTemplates.Any(x => x.Name == originalTemplate.Name) == false)
            {
                originalTemplate.CopyTo(targetTemplatesDirectory.FullName.CombinePath(originalTemplate.Name));
            }
        }
    }

    /// <summary>
    /// Initializes the database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void InitializeDatabase()
    {
        // Establish database connection.
        var dbOptions = _configuration.GetOption<DatabaseOption>();
        var db = new MaaCopilotDbContext(new OptionsWrapper<DatabaseOption>(dbOptions));
        var pendingMigrations = db.Database.GetPendingMigrations().Count();
        if (pendingMigrations > 0)
        {
            db.Database.Migrate();
            Log.Logger.Information("Database migration completed, applied {PendingMigrations} migrations",
                pendingMigrations);
        }

        // Check if there are users in the database.
        var haveUser = db.CopilotUsers.Any();
        if (haveUser is false)
        {
            // New DB without any existing users. Initialize default user.
            var defaultUserEmail = _settings.DefaultUserEmail;
            var defaultUserPassword = _settings.DefaultUserPassword;
            if (defaultUserPassword == "")
            {
                defaultUserPassword = GeneratePassword();
            }

            var defaultUserName = _settings.DefaultUsername;

            if (_settings.IsDefaultUserEmailEmpty || _settings.IsDefaultUserPasswordEmpty)
            {
                Log.Logger.Information("Creating default user with email {DefaultEmail} and password {DefaultPassword}",
                    defaultUserEmail, defaultUserPassword);
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(defaultUserPassword);
            var user = new CopilotUser(defaultUserEmail, hash, defaultUserName, UserRole.SuperAdmin, Guid.Empty);
            user.ActivateUser(Guid.Empty);
            db.CopilotUsers.Add(user);
            db.SaveChanges();
        }

        db.Dispose();
    }

    /// <summary>
    ///     Generates a new password. The generated password matches regexp like <c>^[A-Z]{16}$</c>.
    /// </summary>
    /// <returns>The new password.</returns>
    public static string GeneratePassword()
    {
        var builder = new StringBuilder();
        var random = new Random();
        for (var i = 0; i < 16; i++)
        {
            var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }

        return builder.ToString();
    }
}
