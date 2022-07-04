// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Enum;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Api.Jobs;

public class TokenValidationCheck : IHostedService
{
    private readonly IOptions<DatabaseOption> _dbOptions;
    private readonly ILogger<TokenValidationCheck> _logger;

    private Task? _timedTask;
    private CancellationTokenSource? _cts;

    public TokenValidationCheck(IOptions<DatabaseOption> dbOptions, ILogger<TokenValidationCheck> logger)
    {
        _dbOptions = dbOptions;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Setting up token validation check background task");
        _cts = new CancellationTokenSource();
        _timedTask = RunJob(_cts.Token);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();

        _logger.LogInformation("Waiting for token validation check background task to stop");
        _timedTask?.GetAwaiter().GetResult();
        _logger.LogInformation("Token validation check background task stopped");

        return Task.CompletedTask;
    }

    private async Task RunJob(CancellationToken cancellationToken)
    {
        try
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                if (SystemStatus.IsOk is false)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                    continue;
                }

                var db = new MaaCopilotDbContext(_dbOptions);

                var currentTime = DateTimeOffset.UtcNow;
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

                var expiredTokens = await db.CopilotTokens
                    .Where(x => x.ValidBefore < currentTime)
                    .ToListAsync(cancellationToken);

                expiredTokens.ForEach(x =>
                {
                    x.Delete(Guid.Empty);
                });

                db.CopilotTokens.RemoveRange(expiredTokens);
                await db.SaveChangesAsync(cancellationToken);

                await db.DisposeAsync();

                _logger.LogInformation("MaaCopilotServer: Type -> {LoggingType}; Name -> {Name}; Message -> {Message}",
                    LoggingType.WorkerServicesReport, nameof(TokenValidationCheck), $"{expiredTokens.Count} expired tokens removed");

                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            // Ignore
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "MaaCopilotServer: Type -> {LoggingType}; Name -> {Name}; ExceptionName -> {ExceptionName}",
                LoggingType.WorkerServicesException, nameof(TokenValidationCheck), ex.GetType().Name);
        }
    }
}
