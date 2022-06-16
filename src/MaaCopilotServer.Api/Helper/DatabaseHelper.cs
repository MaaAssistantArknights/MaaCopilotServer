// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace MaaCopilotServer.Api.Helper;

/// <summary>
/// The helper class of database connection.
/// </summary>
public static class DatabaseHelper
{
    /// <summary>
    /// Initializes database based on the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public static void DatabaseInitialize(IConfiguration configuration)
    {
        // Establish database connection.
        var dbOptions = configuration.GetOption<DatabaseOption>();
        var db = new MaaCopilotDbContext(new OptionsWrapper<DatabaseOption>(dbOptions));
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }

        // Check if there are users in the database.
        var haveUser = db.CopilotUsers.Any();
        if (haveUser is false)
        {
            // New DB without any existing users. Initialize default user.
            var defaultUserEmail = Environment.GetEnvironmentVariable("DEFAULT_USER_EMAIL") ?? "super@prts.plus";
            var defaultUserPassword = Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD") ?? GeneratePassword();
            var defaultUserName = Environment.GetEnvironmentVariable("DEFAULT_USER_NAME") ?? "Maa";

            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_USER_EMAIL")) ||
                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DEFAULT_USER_PASSWORD")))
            {
                Log.Logger.Information("Creating default user with email {DefaultEmail} and password {DefaultPassword}",
                    defaultUserEmail, defaultUserPassword);
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(defaultUserPassword);
            var user = new CopilotUser(defaultUserEmail, hash, defaultUserName, UserRole.SuperAdmin, Guid.Parse("00000000-0000-0000-0000-000000000000"));

            // Add changes to DB.
            db.CopilotUsers.Add(user);
            db.SaveChanges();
        }

        db.Dispose();
    }

    /// <summary>
    /// Generates a new password. The generated password matches regexp like <c>^[A-Z]{16}$</c>.
    /// </summary>
    /// <returns>The new password.</returns>
    private static string GeneratePassword()
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
