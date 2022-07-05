// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using System.Text;
using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Constants;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Api.Jobs;

/// <summary>
///     Hosted job for database initialization.
/// </summary>
public class DatabaseInitializer : IHostedService
{
    private readonly IOptions<DatabaseOption> _dbOptions;
    private readonly IOptions<CopilotOperationOption> _copilotOperationOptions;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ICopilotOperationService _copilotOperationService;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        IOptions<DatabaseOption> dbOptions,
        IOptions<CopilotOperationOption> copilotOperationOptions,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<DatabaseInitializer> logger)
    {
        _dbOptions = dbOptions;
        _copilotOperationOptions = copilotOperationOptions;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;

        _copilotOperationService = new CopilotOperationService(copilotOperationOptions, new DomainString());
    }

    private CancellationToken _cancellationToken;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;

        _hostApplicationLifetime.ApplicationStarted.Register(Initialize);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void Initialize()
    {
        var db = new MaaCopilotDbContext(_dbOptions);

        InitializeMigration(db, _cancellationToken).GetAwaiter().GetResult();
        InitializeDefaultUser(db, _cancellationToken).GetAwaiter().GetResult();
        InitializeCheckOperation(db, _cancellationToken).GetAwaiter().GetResult();

        db.Dispose();

        _logger.LogInformation("Database initialization finished");
        SystemStatus.DatabaseInitialized = true;
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

    private async Task InitializeMigration(DbContext dbContext, CancellationToken cancellationToken)
    {
        var pendingMigrations = (await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Count();
        if (pendingMigrations > 0)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migration completed, applied {PendingMigrations} migrations",
                pendingMigrations);
        }
    }

    private async Task InitializeDefaultUser(MaaCopilotDbContext dbContext, CancellationToken cancellationToken)
    {
        var haveUser = dbContext.CopilotUsers.Any();
        if (haveUser is false)
        {
            // New DB without any existing users. Initialize default user.
            var defaultUserEmail = GlobalConstants.DefaultUserEmail;
            var defaultUserPassword = GlobalConstants.DefaultUserPassword;
            if (defaultUserPassword == "")
            {
                defaultUserPassword = GeneratePassword();
            }

            var defaultUserName = GlobalConstants.DefaultUsername;

            if (GlobalConstants.IsDefaultUserEmailEmpty || GlobalConstants.IsDefaultUserPasswordEmpty)
            {
                _logger.LogInformation("Creating default user with email {DefaultEmail} and password {DefaultPassword}",
                    defaultUserEmail, defaultUserPassword);
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(defaultUserPassword);
            var user = new CopilotUser(defaultUserEmail, hash, defaultUserName, UserRole.SuperAdmin, Guid.Empty);
            user.ActivateUser(Guid.Empty);
            dbContext.CopilotUsers.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task InitializeCheckOperation(MaaCopilotDbContext dbContext, CancellationToken cancellationToken)
    {
        var likeMultiplier = await dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.HOT_LIKE_MULTIPLIER, cancellationToken);
        var dislikeMultiplier = await dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.HOT_DISLIKE_MULTIPLIER, cancellationToken);
        var viewMultiplier = await dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.HOT_VIEW_MULTIPLIER, cancellationToken);
        var initialScore = await dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.HOT_INITIAL_SCORE, cancellationToken);

        var currentLikeMultiplier = _copilotOperationOptions.Value.LikeMultiplier.ToString(new CultureInfo("zh-CN"));
        var currentDislikeMultiplier = _copilotOperationOptions.Value.DislikeMultiplier.ToString(new CultureInfo("zh-CN"));
        var currentViewMultiplier = _copilotOperationOptions.Value.ViewMultiplier.ToString(new CultureInfo("zh-CN"));
        var currentInitialScore = _copilotOperationOptions.Value.InitialHotScore.ToString(new CultureInfo("zh-CN"));

        var refreshRequired = false;

        #region Check HOT SCORE calculation parameters

        if (likeMultiplier is null)
        {
            likeMultiplier = new PersistStorage(SystemConstants.HOT_LIKE_MULTIPLIER, currentLikeMultiplier);
            dbContext.PersistStorage.Add(likeMultiplier);
            refreshRequired = true;
        }
        else if (likeMultiplier.Value != currentLikeMultiplier)
        {
            likeMultiplier.UpdateValue(currentLikeMultiplier);
            refreshRequired = true;
        }

        if (dislikeMultiplier is null)
        {
            dislikeMultiplier = new PersistStorage(SystemConstants.HOT_DISLIKE_MULTIPLIER, currentDislikeMultiplier);
            dbContext.PersistStorage.Add(dislikeMultiplier);
            refreshRequired = true;
        }
        else if (dislikeMultiplier.Value != currentDislikeMultiplier)
        {
            dislikeMultiplier.UpdateValue(currentDislikeMultiplier);
            refreshRequired = true;
        }

        if (viewMultiplier is null)
        {
            viewMultiplier = new PersistStorage(SystemConstants.HOT_VIEW_MULTIPLIER, currentViewMultiplier);
            dbContext.PersistStorage.Add(viewMultiplier);
            refreshRequired = true;
        }
        else if (viewMultiplier.Value != currentViewMultiplier)
        {
            viewMultiplier.UpdateValue(currentViewMultiplier);
            refreshRequired = true;
        }

        if (initialScore is null)
        {
            initialScore = new PersistStorage(SystemConstants.HOT_INITIAL_SCORE, currentInitialScore);
            dbContext.PersistStorage.Add(initialScore);
            refreshRequired = true;
        }
        else if (initialScore.Value != currentInitialScore)
        {
            initialScore.UpdateValue(currentInitialScore);
            refreshRequired = true;
        }

        #endregion

        if (refreshRequired is false)
        {
            return;
        }

        var count = await dbContext.CopilotOperations.CountAsync(cancellationToken);

        var hasNext = true;
        var page = 1;
        var changes = 0;
        while (hasNext)
        {
            var opers = await dbContext.CopilotOperations
                .OrderBy(x => x.Id)
                .Skip((page - 1) * 1000)
                .Take(1000)
                .ToListAsync(cancellationToken);

            hasNext = page * 1000 < count;

            opers.ForEach(x =>
            {
                x.UpdateHotScore(_copilotOperationService.CalculateHotScore(x));
                dbContext.Update(x);
            });

            var dbChanges = await dbContext.SaveChangesAsync(cancellationToken);

            changes += dbChanges;
            page++;
        }

        _logger.LogInformation("InitializeCheckOperation completed, total changes: {DbInitializeChanges}", changes);
    }
}
