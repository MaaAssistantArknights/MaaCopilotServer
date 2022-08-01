// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Enum;
using MaaCopilotServer.Domain.Constants;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.GameData;
using MaaCopilotServer.GameData.Exceptions;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ArkServerLanguage = MaaCopilotServer.GameData.Constants.ArkServerLanguage;

namespace MaaCopilotServer.Api.Jobs;

public class ArknightsDataUpdate : IHostedService
{
    private readonly ILogger<ArknightsDataUpdate> _logger;
    private readonly IOptions<CopilotServerOption> _copilotServerOptions;
    private readonly IOptions<DatabaseOption> _dbOptions;

    private readonly Action<Exception> _exceptionLogger;

    private Task? _timedTask;
    private CancellationTokenSource? _cts;

    public ArknightsDataUpdate(
        ILogger<ArknightsDataUpdate> logger,
        IOptions<CopilotServerOption> copilotServerOptions,
        IOptions<DatabaseOption> dbOptions)
    {
        _logger = logger;
        _copilotServerOptions = copilotServerOptions;
        _dbOptions = dbOptions;

        _cts = new CancellationTokenSource();

        _exceptionLogger = ex =>
        {
            if (ex is GameDataParseException)
            {
                return;
            }

            _logger.LogError(ex,
                "MaaCopilotServer: Type -> {LoggingType}; Name -> {Name}; ExceptionName -> {ExceptionName}",
                LoggingType.WorkerServicesException, nameof(TokenValidationCheck), ex.GetType().Name);
        };
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Setting up arknights data update background task");
        _cts = new CancellationTokenSource();
        _timedTask = RunJob(_cts.Token);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();

        _logger.LogInformation("Waiting for arknights data update background task to stop");
        _timedTask?.GetAwaiter().GetResult();
        _logger.LogInformation("Arknights data update background task stopped");

        return Task.CompletedTask;
    }

    private async Task RunJob(CancellationToken cancellationToken)
    {
        try
        {
            while (cancellationToken.IsCancellationRequested is false)
            {
                while (SystemStatus.DatabaseInitialized is false)
                {
                    await Task.Delay(5000, cancellationToken);
                }

                var db = new MaaCopilotDbContext(_dbOptions);
                var versions = await GetDataVersion();

                var levelVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_LEVEL);
                var cnVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_CN);
                var enVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_EN);
                var jpVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_JP);
                var koVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_KO);

                var updateRequired = false;

                #region Check if update is required or not

                if (levelVersion is null)
                {
                    levelVersion = new PersistStorage(SystemConstants.ARK_ASSET_VERSION_LEVEL, versions.Level);
                    db.PersistStorage.Add(levelVersion);
                    updateRequired = true;
                }
                else if (levelVersion.Value != versions.Level)
                {
                    levelVersion.UpdateValue(versions.Level);
                    db.PersistStorage.Update(levelVersion);
                    updateRequired = true;
                }

                if (cnVersion is null)
                {
                    cnVersion = new PersistStorage(SystemConstants.ARK_ASSET_VERSION_CN, versions.Cn);
                    db.PersistStorage.Add(cnVersion);
                    updateRequired = true;
                }
                else if (cnVersion.Value != versions.Cn)
                {
                    cnVersion.UpdateValue(versions.Cn);
                    db.PersistStorage.Update(cnVersion);
                    updateRequired = true;
                }

                if (enVersion is null)
                {
                    enVersion = new PersistStorage(SystemConstants.ARK_ASSET_VERSION_EN, versions.En);
                    db.PersistStorage.Add(enVersion);
                    updateRequired = true;
                }
                else if (enVersion.Value != versions.En)
                {
                    enVersion.UpdateValue(versions.En);
                    db.PersistStorage.Update(enVersion);
                    updateRequired = true;
                }

                if (jpVersion is null)
                {
                    jpVersion = new PersistStorage(SystemConstants.ARK_ASSET_VERSION_JP, versions.Jp);
                    db.PersistStorage.Add(jpVersion);
                    updateRequired = true;
                }
                else if (jpVersion.Value != versions.Jp)
                {
                    jpVersion.UpdateValue(versions.Jp);
                    db.PersistStorage.Update(jpVersion);
                    updateRequired = true;
                }

                if (koVersion is null)
                {
                    koVersion = new PersistStorage(SystemConstants.ARK_ASSET_VERSION_KO, versions.Ko);
                    db.PersistStorage.Add(koVersion);
                    updateRequired = true;
                }
                else if (koVersion.Value != versions.Ko)
                {
                    koVersion.UpdateValue(versions.Ko);
                    db.PersistStorage.Update(koVersion);
                    updateRequired = true;
                }

                #endregion

                if (updateRequired is false)
                {
                    SystemStatus.ArknightsDataInitialized = true;
                    return;
                }

                var dbLevelData = await db.ArkLevelData.ToListAsync(cancellationToken);
                var dbCharData = await db.ArkCharacterInfos.ToListAsync(cancellationToken);
                var currentData = await GetParsedData();

                var newLevelData = new List<ArkLevelData>();
                var newCharData = new List<ArkCharacterInfo>();

                foreach (var e in currentData.ArkLevelEntities)
                {
                    var dbE = dbLevelData.FirstOrDefault(x => x.LevelId == e.LevelId);
                    if (dbE is null)
                    {
                        newLevelData.Add(new ArkLevelData(e));
                        continue;
                    }

                    if (dbE.IsEqual(e))
                    {
                        continue;
                    }

                    dbE.Update(e);
                    db.ArkLevelData.Update(dbE);
                }

                foreach (var e in currentData.ArkCharacterInfos)
                {
                    var dbE = dbCharData.FirstOrDefault(x => x.Id == e.Id);
                    if (dbE is null)
                    {
                        newCharData.Add(new ArkCharacterInfo(e));
                        continue;
                    }

                    if (dbE.IsEqual(e))
                    {
                        continue;
                    }

                    dbE.Update(e);
                    db.ArkCharacterInfos.Update(dbE);
                }

                db.ArkLevelData.AddRange(newLevelData);
                db.ArkCharacterInfos.AddRange(newCharData);

                var changes = await db.SaveChangesAsync(cancellationToken);

                await db.DisposeAsync();

                _logger.LogInformation("MaaCopilotServer: Type -> {LoggingType}; Name -> {Name}; Message -> {Message}",
                    LoggingType.WorkerServicesReport, nameof(ArknightsDataUpdate),
                    $"{changes} changes saved to database");

                SystemStatus.ArknightsDataInitialized = true;

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
                LoggingType.WorkerServicesException, nameof(ArknightsDataUpdate), ex.GetType().Name);
        }
    }

    private async Task<ArkDataParsed> GetParsedData()
    {
        var handler = new HttpClientHandler();
        handler.AllowAutoRedirect = true;
        handler.UseProxy = _copilotServerOptions.Value.GitHubApiRequestProxyEnable;
        handler.Proxy = handler.UseProxy
            ? new WebProxy(_copilotServerOptions.Value.GitHubApiRequestProxyAddress, _copilotServerOptions.Value.GitHubApiRequestProxyPort)
            : null;
        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("User-Agent", _copilotServerOptions.Value.GitHubApiRequestUserAgent);

        var level = await client.GetStringAsync(new Uri(SystemConstants.LevelUrl)).ConfigureAwait(true);

        var ds = new List<ArkDataSource>();

        foreach (var region in SystemConstants.ArkServerRegions)
        {
            var act = await client.GetStringAsync(new Uri(SystemConstants.ActUrl.Replace("{REGION}", region.Key,
                StringComparison.Ordinal))).ConfigureAwait(true);
            var character = await client.GetStringAsync(new Uri(SystemConstants.CharUrl.Replace("{REGION}", region.Key,
                StringComparison.Ordinal))).ConfigureAwait(true);
            var stage = await client.GetStringAsync(new Uri(SystemConstants.StageUrl.Replace("{REGION}", region.Key,
                StringComparison.Ordinal))).ConfigureAwait(true);
            var zone = await client.GetStringAsync(new Uri(SystemConstants.ZoneUrl.Replace("{REGION}", region.Key,
                StringComparison.Ordinal))).ConfigureAwait(true);

            ds.Add(new ArkDataSource
            {
                ArkAct = act,
                ArkChar = character,
                ArkZone = zone,
                ArkStage = stage,
                ArkLevel = level,
                Language = region.Value,
            });
        }

        handler.Dispose();
        client.Dispose();

        var cnS = ds.First(x => x.Language == ArkServerLanguage.Chinese);
        var enS = ds.First(x => x.Language == ArkServerLanguage.English);
        var jpS = ds.First(x => x.Language == ArkServerLanguage.Japanese);
        var koS = ds.First(x => x.Language == ArkServerLanguage.Korean);
        var p = GameDataParser.Parse(cnS, enS, jpS, koS, _exceptionLogger);

        return p;
    }

    private async Task<ArkDataVersions> GetDataVersion()
    {
        var handler = new HttpClientHandler();
        handler.AllowAutoRedirect = true;
        handler.UseProxy = _copilotServerOptions.Value.GitHubApiRequestProxyEnable;
        handler.Proxy = handler.UseProxy
            ? new WebProxy(_copilotServerOptions.Value.GitHubApiRequestProxyAddress,
                _copilotServerOptions.Value.GitHubApiRequestProxyPort)
            : null;
        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("User-Agent", _copilotServerOptions.Value.GitHubApiRequestUserAgent);

        var level = await client.GetStringAsync(new Uri(SystemConstants.ArkLevelCommit)).ConfigureAwait(true);

        var levelSha = GetCommitSha(level);
        var cnSha = "";
        var enSha = "";
        var jpSha = "";
        var koSha = "";

        foreach (var (region, language) in SystemConstants.ArkServerRegions)
        {
            var url = SystemConstants.ArkDataCommit.Replace("{REGION}", region, StringComparison.Ordinal);
            var data = await client.GetStringAsync(new Uri(url)).ConfigureAwait(true);

            switch (language)
            {
                case ArkServerLanguage.Chinese:
                    cnSha = GetCommitSha(data);
                    break;
                case ArkServerLanguage.Korean:
                    koSha = GetCommitSha(data);
                    break;
                case ArkServerLanguage.English:
                    enSha = GetCommitSha(data);
                    break;
                case ArkServerLanguage.Japanese:
                    jpSha = GetCommitSha(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        handler.Dispose();
        client.Dispose();

        return new ArkDataVersions(cnSha, enSha, jpSha, koSha, levelSha);
    }

    private static string GetCommitSha(string str)
    {
        var obj = JsonSerializer.Deserialize<List<GitHubCommitApiResponse>>(str);
        if (obj is null || obj.Count == 0)
        {
            throw new Exception("Could not get commit sha");
        }

        return obj.First().Sha;
    }

    private record ArkDataVersions(string Cn, string En, string Jp, string Ko, string Level);

    private record GitHubCommitApiResponse
    {
        [JsonPropertyName("sha")] public string Sha { get; init; } = string.Empty;
    }
}
