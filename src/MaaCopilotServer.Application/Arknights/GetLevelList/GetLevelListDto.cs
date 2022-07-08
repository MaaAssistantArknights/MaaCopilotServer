// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Arknights.GetLevelList;

/// <summary>
///     The response of the get level list query.
/// </summary>
public class GetLevelListDto
{
    /// <summary>
    ///     The constructor of <see cref="GetLevelListDto"/>
    /// </summary>
    /// <param name="catOne">Category one of the level.</param>
    /// <param name="catTwo">Category two of the level.</param>
    /// <param name="catThree">Category three of the level.</param>
    /// <param name="name">The name of the level.</param>
    /// <param name="levelId">The level id.</param>
    /// <param name="width">The width of the level.</param>
    /// <param name="height">The height of the level.</param>
    public GetLevelListDto(string catOne, string catTwo, string catThree, string name, string levelId, int width, int height)
    {
        CatOne = catOne;
        CatTwo = catTwo;
        CatThree = catThree;
        Name = name;
        LevelId = levelId;
        Width = width;
        Height = height;
    }

#pragma warning disable CS8618
    public GetLevelListDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     Category one of the level.
    /// </summary>
    [Required]
    [JsonPropertyName("cat_one")]
    public string CatOne { get; set; }

    /// <summary>
    ///     Category two of the level.
    /// </summary>
    [Required]
    [JsonPropertyName("cat_two")]
    public string CatTwo { get; set; }

    /// <summary>
    ///     Category three of the level.
    /// </summary>
    [Required]
    [JsonPropertyName("cat_three")]
    public string CatThree { get; set; }

    /// <summary>
    ///     The name of the level.
    /// </summary>
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     The level id.
    /// </summary>
    [Required]
    [JsonPropertyName("level_id")]
    public string LevelId { get; set; }

    /// <summary>
    ///     The width of the level.
    /// </summary>
    [Required]
    [JsonPropertyName("width")]
    public int Width { get; set; }

    /// <summary>
    ///     The height of the level.
    /// </summary>
    [Required]
    [JsonPropertyName("height")]
    public int Height { get; set; }
}
