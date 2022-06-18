// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUserFavorites;

public class GetCopilotUserFavoritesDto
{
    public GetCopilotUserFavoritesDto(string favoriteListId, string favoriteListName,
        List<QueryCopilotOperationsQueryDto> favoriteItems)
    {
        FavoriteListId = favoriteListId;
        FavoriteListName = favoriteListName;
        FavoriteItems = favoriteItems;
    }

#pragma warning disable CS8618
    public GetCopilotUserFavoritesDto() { }
#pragma warning restore CS8618

    [JsonPropertyName("favorite_list_id")] public string FavoriteListId { get; set; }

    [JsonPropertyName("favorite_list_name")]
    public string FavoriteListName { get; set; }

    [JsonPropertyName("favorite_items")] public List<QueryCopilotOperationsQueryDto> FavoriteItems { get; set; }
}
