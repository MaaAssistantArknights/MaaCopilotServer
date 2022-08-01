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
    /// <param name="cnVersion">Chinese (China Mainland) server data version.</param>
    /// <param name="twVersion">Chinese (Taiwan, China) server data version.</param>
    /// <param name="enVersion">English (Global) server data version.</param>
    /// <param name="jpVersion">Japanese (Japan) server data version.</param>
    /// <param name="koVersion">Korean (Korea) server data version.</param>
    public GetDataVersionQueryDto(string levelVersion, string cnVersion, string twVersion, string enVersion, string jpVersion, string koVersion)
    {
        LevelVersion = levelVersion;
        CnVersion = cnVersion;
        TwVersion = twVersion;
        EnVersion = enVersion;
        JpVersion = jpVersion;
        KoVersion = koVersion;
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
    ///     Chinese (China Mainland) server data version.
    /// </summary>
    [Required]
    [JsonPropertyName("chinese_simplified")]
    public string CnVersion { get; set; }
    
    /// <summary>
    ///     Chinese (Taiwan, China) server data version.
    /// </summary>
    [Required]
    [JsonPropertyName("chinese_traditional")]
    public string TwVersion { get; set; }

    /// <summary>
    ///     English (Global) server data version.
    /// </summary>
    [Required]
    [JsonPropertyName("english")]
    public string EnVersion { get; set; }

    /// <summary>
    ///     Japanese (Japan) server data version.
    /// </summary>
    [Required]
    [JsonPropertyName("japanese")]
    public string JpVersion { get; set; }

    /// <summary>
    ///     Korean (Korea) server data version.
    /// </summary>
    [Required]
    [JsonPropertyName("korean")]
    public string KoVersion { get; set; }
}
