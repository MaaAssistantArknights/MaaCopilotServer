// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Arknights.GetDataVersion;

/// <summary>
///     The response to the <see cref="GetDataVersionQuery"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class GetDataVersionQueryDto
{
    /// <summary>
    ///     The constructor of <see cref="GetDataVersionQueryDto"/>.
    /// </summary>
    /// <param name="levelVersion">Level version.</param>
    /// <param name="serverVersion">Server data version.</param>
    /// <param name="serverSyncStatus">Server data sync status.</param>
    public GetDataVersionQueryDto(string levelVersion, ServerStatusDto serverVersion, ServerStatusDto serverSyncStatus)
    {
        LevelVersion = levelVersion;
        ServerVersion = serverVersion;
        ServerSyncStatus = serverSyncStatus;
    }

#pragma warning disable CS8618
    public GetDataVersionQueryDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     Level version.
    /// </summary>
    [Required]
    [JsonPropertyName("level")]
    public string LevelVersion { get; set; }

    /// <summary>
    ///     Server data version.
    /// </summary>
    [Required]
    [JsonPropertyName("version")]
    public ServerStatusDto ServerVersion { get; set; }

    /// <summary>
    ///     Server data sync status.
    /// </summary>
    [Required]
    [JsonPropertyName("status")]
    public ServerStatusDto ServerSyncStatus { get; set; }
}

/// <summary>
///     Server specific data.
/// </summary>
public record ServerStatusDto
{
    /// <summary>
    ///     The constructor of <see cref="ServerStatusDto"/>.
    /// </summary>
    /// <param name="chineseSimplified">CN Mainland</param>
    /// <param name="chineseTraditional">CN TW</param>
    /// <param name="english">EN Global</param>
    /// <param name="japanese">JA Japan</param>
    /// <param name="korean">KO Korea</param>
    public ServerStatusDto(string chineseSimplified, string chineseTraditional, string english, string japanese, string korean)
    {
        ChineseSimplified = chineseSimplified;
        ChineseTraditional = chineseTraditional;
        English = english;
        Japanese = japanese;
        Korean = korean;
    }

    /// <summary>
    ///     The constructor of <see cref="ServerStatusDto"/>.
    /// </summary>
    /// <param name="same">Same string for every properties.</param>
    public ServerStatusDto(string same)
    {
        ChineseSimplified = same;
        ChineseTraditional = same;
        English = same;
        Japanese = same;
        Korean = same;
    }

    /// <summary>
    ///     Chinese (China Mainland) server info.
    /// </summary>
    [Required]
    [JsonPropertyName("chinese_simplified")]
    public string ChineseSimplified { get; set; }

    /// <summary>
    ///     Chinese (Taiwan, China) server info.
    /// </summary>
    [Required]
    [JsonPropertyName("chinese_traditional")]
    public string ChineseTraditional { get; set; }

    /// <summary>
    ///     English (Global) server info.
    /// </summary>
    [Required]
    [JsonPropertyName("english")]
    public string English { get; set; }

    /// <summary>
    ///     Japanese (Japan) server info.
    /// </summary>
    [Required]
    [JsonPropertyName("japanese")]
    public string Japanese { get; set; }

    /// <summary>
    ///     Korean (Korea) server info.
    /// </summary>
    [Required]
    [JsonPropertyName("korean")]
    public string Korean { get; set; }
}
