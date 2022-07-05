// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Arknights.GetOperatorList;

/// <summary>
///     The response of the <see cref="GetOperatorListQuery"/>.
/// </summary>
public class GetOperatorListDto
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <param name="profession"></param>
    /// <param name="star"></param>
    public GetOperatorListDto(string name, string id, string profession, int star)
    {
        Name = name;
        Id = id;
        Profession = profession;
        Star = star;
    }

#pragma warning disable CS8618
    public GetOperatorListDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The name of the operator.
    /// </summary>
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     The ID of the operator.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The profession of the operator.
    /// </summary>
    [Required]
    [JsonPropertyName("profession")]
    public string Profession { get; set; }

    /// <summary>
    ///     The rarity of the operator.
    /// </summary>
    [Required]
    [JsonPropertyName("star")]
    public int Star { get; set; }
}
