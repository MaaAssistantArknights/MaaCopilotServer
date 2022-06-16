// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

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

public static class InitializeHelper
{
    public static void InitializeEmailTemplates(IConfiguration configuration)
    {
        var applicationOption = configuration.GetOption<ApplicationOption>();
        var originalTemplatesDirectory = new DirectoryInfo(applicationOption.AssemblyPath.CombinePath("templates"));
        var targetTemplatesDirectory = new DirectoryInfo(applicationOption.DataDirectory.CombinePath("templates"));

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

    public static void InitializeDatabase(IConfiguration configuration)
    {
        var dbOptions = configuration.GetOption<DatabaseOption>();
        var db = new MaaCopilotDbContext(new OptionsWrapper<DatabaseOption>(dbOptions));
        var pendingMigrations = db.Database.GetPendingMigrations().Count();
        if (pendingMigrations > 0)
        {
            db.Database.Migrate();
            Log.Logger.Information("Database migration completed, applied {PendingMigrations} migrations", pendingMigrations);
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
            var user = new CopilotUser(defaultUserEmail, hash, defaultUserName, UserRole.SuperAdmin,
                Guid.Parse("00000000-0000-0000-0000-000000000000"));
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