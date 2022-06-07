// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MaaCopilotServer.Api.Helper;

public static class DatabaseHelper
{
    public static void DatabaseInitialize(IServiceProvider provider, IConfiguration configuration)
    {
        var db = new MaaCopilotDbContext(configuration);
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }

        var haveUser = db.CopilotUsers.Any();
        if (haveUser is false)
        {
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
            var user = new CopilotUser(defaultUserEmail, hash, defaultUserName, UserRole.Admin);
            db.CopilotUsers.Add(user);
            db.SaveChanges();
        }

        db.Dispose();
    }

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
