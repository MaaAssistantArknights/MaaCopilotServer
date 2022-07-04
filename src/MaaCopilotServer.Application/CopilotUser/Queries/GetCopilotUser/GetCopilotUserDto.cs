// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

/// <summary>
///     The response to the <see cref="GetCopilotUserQuery"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class GetCopilotUserDto
{
    /// <summary>
    ///     The constructor of <see cref="GetCopilotUserDto" />.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="userName">The username.</param>
    /// <param name="userRole">The role of the user.</param>
    /// <param name="uploadCount">The number of uploads.</param>
    /// <param name="accountActivated">Account activation status.</param>
    /// <param name="favoriteLists">The list of favorite list.</param>
    public GetCopilotUserDto(Guid id, string userName, UserRole userRole, int uploadCount, bool accountActivated,
        Dictionary<string, string> favoriteLists)
    {
        Id = id;
        UserName = userName;
        UserRole = userRole;
        UploadCount = uploadCount;
        AccountActivated = accountActivated;
        FavoriteLists = favoriteLists;
    }

#pragma warning disable CS8618
    public GetCopilotUserDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The user id.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     The username.
    /// </summary>
    [Required]
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    /// <summary>
    ///     The role of the user.
    /// </summary>
    [Required]
    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole UserRole { get; set; }

    /// <summary>
    ///     Account activation status.
    /// </summary>
    [Required]
    [JsonPropertyName("activated")]
    public bool AccountActivated { get; set; }

    /// <summary>
    ///     User favorite lists.
    /// </summary>
    [Required]
    [JsonPropertyName("favorite_lists")]
    public Dictionary<string, string> FavoriteLists { get; set; }

    /// <summary>
    ///     The number of uploads.
    /// </summary>
    [Required]
    [JsonPropertyName("upload_count")]
    public int UploadCount { get; set; }
}
