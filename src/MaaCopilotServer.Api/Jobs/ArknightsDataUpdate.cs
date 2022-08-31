// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using System.Text.Json.Serialization;
using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Enum;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Constants;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.GameData;
using MaaCopilotServer.GameData.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ArkServerLanguage = MaaCopilotServer.GameData.Constants.ArkServerLanguage;

namespace MaaCopilotServer.Api.Jobs;

public class ArknightsDataUpdate : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ArknightsDataUpdate> _logger;
    private readonly IOptions<CopilotServerOption> _copilotServerOptions;

    /// <summary>
    /// The HTTP client factory.
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly Action<Exception, ArkServerLanguage> _exceptionLogger;

    private readonly List<ArkServerLanguage> _syncErrors = new();
    private bool _disaster;

    private Task? _timedTask;
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArknightsDataUpdate"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="logger">The logger,</param>
    /// <param name="copilotServerOptions">The copilot server options.</param>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    public ArknightsDataUpdate(
        IServiceProvider serviceProvider,
        ILogger<ArknightsDataUpdate> logger,
        IOptions<CopilotServerOption> copilotServerOptions,
        IHttpClientFactory httpClientFactory)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _copilotServerOptions = copilotServerOptions;
        _httpClientFactory = httpClientFactory;

        _cts = new CancellationTokenSource();

        _exceptionLogger = (ex, lang) =>
        {
            if (ex is GameDataParseException)
            {
                return;
            }

            if (_syncErrors.Contains(lang) is false)
            {
                _syncErrors.Add(lang);
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
        while (cancellationToken.IsCancellationRequested is false)
        {
            var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IMaaCopilotDbContext>();

            try
            {
                while (SystemStatus.DatabaseInitialized is false)
                {
                    await Task.Delay(5000, cancellationToken);
                }

                var versions = await GetDataVersion();

                var levelVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_LEVEL);
                var cnVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_CN);
                var twVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_TW);
                var enVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_EN);
                var jpVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_JP);
                var koVersion =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_VERSION_KO);
                var error =
                    db.PersistStorage.FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_CACHE_ERROR);

                var updateRequired = string.IsNullOrEmpty(error?.Value);

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

                if (twVersion is null)
                {
                    twVersion = new PersistStorage(SystemConstants.ARK_ASSET_VERSION_TW, versions.Tw);
                    db.PersistStorage.Add(twVersion);
                    updateRequired = true;
                }
                else if (twVersion.Value != versions.Tw)
                {
                    twVersion.UpdateValue(versions.Tw);
                    db.PersistStorage.Update(twVersion);
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

                var dbLevelData = await db.ArkLevelData
                    .Where(x => x.Custom == false)
                    .ToListAsync(cancellationToken);
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

                var errorStr = string.Join(";", _syncErrors.Select(x => x.ToString()));

                _syncErrors.Clear();

                var store = db.PersistStorage
                    .FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_CACHE_ERROR);

                if (store is null)
                {
                    store = new PersistStorage(SystemConstants.ARK_ASSET_CACHE_ERROR, errorStr);
                    db.PersistStorage.Add(store);
                }
                else
                {
                    store.UpdateValue(errorStr);
                    db.PersistStorage.Update(store);
                }

                var changes = await db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("MaaCopilotServer: Type -> {LoggingType}; Name -> {Name}; Message -> {Message}",
                    LoggingType.WorkerServicesReport, nameof(ArknightsDataUpdate),
                    $"{changes} changes saved to database");

                SystemStatus.ArknightsDataInitialized = true;

                await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
            }
            catch (TaskCanceledException)
            {
                // Ignore
            }
            catch (Exception ex)
            {
                _disaster = true;

                _logger.LogCritical(ex,
                    "MaaCopilotServer: Type -> {LoggingType}; Name -> {Name}; ExceptionName -> {ExceptionName}",
                    LoggingType.WorkerServicesException, nameof(ArknightsDataUpdate), ex.GetType().Name);
            }
            finally
            {
                // ReSharper disable once InvertIf
                if (_disaster)
                {
                    _disaster = false;

                    var store = db.PersistStorage
                        .FirstOrDefault(x => x.Key == SystemConstants.ARK_ASSET_CACHE_ERROR);

                    if (store is null)
                    {
                        store = new PersistStorage(SystemConstants.ARK_ASSET_CACHE_ERROR, SystemConstants.ARK_ASSET_CACHE_ERROR_DISASTER);
                        db.PersistStorage.Add(store);
                    }
                    else
                    {
                        store.UpdateValue(SystemConstants.ARK_ASSET_CACHE_ERROR_DISASTER);
                        db.PersistStorage.Update(store);
                    }
                    await db.SaveChangesAsync(cancellationToken);
                }
                
                scope.Dispose();
            }
        }
    }

    private async Task<ArkDataParsed> GetParsedData()
    {
        using var client = _httpClientFactory.CreateClient(GlobalConstants.GitHubApiProxiedHttpClient);
        client.DefaultRequestHeaders.Add("User-Agent", _copilotServerOptions.Value.GitHubApiRequestUserAgent);
        client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        client.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");

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

        var cnS = ds.First(x => x.Language == ArkServerLanguage.ChineseSimplified);
        var cnTs = ds.First(x => x.Language == ArkServerLanguage.ChineseTraditional);
        var enS = ds.First(x => x.Language == ArkServerLanguage.English);
        var jpS = ds.First(x => x.Language == ArkServerLanguage.Japanese);
        var koS = ds.First(x => x.Language == ArkServerLanguage.Korean);
        var p = GameDataParser.Parse(cnS, cnTs, enS, jpS, koS, _exceptionLogger);

        return p;
    }

    private async Task<ArkDataVersions> GetDataVersion()
    {
        using var client = _httpClientFactory.CreateClient(GlobalConstants.GitHubApiProxiedHttpClient);
        client.DefaultRequestHeaders.Add("User-Agent", _copilotServerOptions.Value.GitHubApiRequestUserAgent);

        var level = await client.GetStringAsync(new Uri(SystemConstants.ArkLevelCommit)).ConfigureAwait(true);

        var levelSha = GetCommitSha(level);
        var cnSha = "";
        var twSha = "";
        var enSha = "";
        var jpSha = "";
        var koSha = "";

        foreach (var (region, language) in SystemConstants.ArkServerRegions)
        {
            var url = SystemConstants.ArkDataCommit.Replace("{REGION}", region, StringComparison.Ordinal);
            var data = await client.GetStringAsync(new Uri(url)).ConfigureAwait(true);

            switch (language)
            {
                case ArkServerLanguage.ChineseSimplified:
                    cnSha = GetCommitSha(data);
                    break;
                case ArkServerLanguage.ChineseTraditional:
                    twSha = GetCommitSha(data);
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

        return new ArkDataVersions(cnSha, twSha, enSha, jpSha, koSha, levelSha);
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

    private record ArkDataVersions(string Cn, string Tw, string En, string Jp, string Ko, string Level);

    private record GitHubCommitApiResponse
    {
        [JsonPropertyName("sha")] public string Sha { get; init; } = string.Empty;
    }
}
