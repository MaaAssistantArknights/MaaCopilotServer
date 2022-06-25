// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUserFavorites;

public class GetCopilotUserFavoritesDto
{
    public GetCopilotUserFavoritesDto(
        string favoriteListId,
        string favoriteListName,
        List<FavoriteCopilotOperationsDto> favoriteItems)
    {
        FavoriteListId = favoriteListId;
        FavoriteListName = favoriteListName;
        FavoriteItems = favoriteItems;
    }

#pragma warning disable CS8618
    public GetCopilotUserFavoritesDto() { }
#pragma warning restore CS8618

    [JsonPropertyName("favorite_list_id")]
    public string FavoriteListId { get; set; }

    [JsonPropertyName("favorite_list_name")]
    public string FavoriteListName { get; set; }

    [JsonPropertyName("favorite_items")]
    public List<FavoriteCopilotOperationsDto> FavoriteItems { get; set; }
}

public class FavoriteCopilotOperationsDto : QueryCopilotOperationsQueryDto
{
    /// <summary>
    ///     The constructor of <see cref="QueryCopilotOperationsQueryDto" />.
    /// </summary>
    /// <param name="id">The operation ID.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="uploadTime">The time when the operation was uploaded.</param>
    /// <param name="uploader">The name of the uploader.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="detail">The detail of the operation.</param>
    /// <param name="viewCounts">The view counts of the operation.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="deleted">Whether this operation has been deleted or not.</param>
    public FavoriteCopilotOperationsDto(
        string id,
        string stageName,
        string minimumRequired,
        string uploadTime,
        string uploader,
        string title,
        string detail,
        int viewCounts,
        List<string> operators,
        bool deleted = false)
        : base(id, stageName,  minimumRequired,  uploadTime, uploader,  title,  detail, viewCounts,operators)
    {
        Id = id;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        UploadTime = uploadTime;
        Uploader = uploader;
        Title = title;
        Detail = detail;
        ViewCounts = viewCounts;
        Operators = operators;
        Deleted = deleted;
    }

#pragma warning disable CS8618
    public FavoriteCopilotOperationsDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     Whether this operation has been deleted or not.
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}
