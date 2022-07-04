// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.CreateFavoriteList;

/// <summary>
///     The response to the <see cref="CreateFavoriteListCommand" />.
/// </summary>
[ExcludeFromCodeCoverage]
public record CreateFavoriteListDto
{
    public CreateFavoriteListDto(string name, string id)
    {
        Name = name;
        Id = id;
    }

#pragma warning disable CS8618
    public CreateFavoriteListDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The name of the favorite list.
    /// </summary>
    [Required]
    [JsonPropertyName("favorite_list_name")]
    public string Name { get; init; }

    /// <summary>
    ///     The id of the favorite list. GUID format.
    /// </summary>
    [Required]
    [JsonPropertyName("favorite_list_id")]
    public string Id { get; set; }
}
