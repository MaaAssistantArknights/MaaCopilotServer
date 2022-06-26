// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Maa Copilot operation entity.
/// </summary>
public sealed class CopilotOperation : EditableEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperation" />.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="author">The author of the operation.</param>
    /// <param name="createBy">The creator of the operation.</param>
    /// <param name="operators">The operators in the operation.</param>
    public CopilotOperation(string content, string stageName, string minimumRequired, string title, string details,
        CopilotUser author, Guid createBy, List<string> operators)
    {
        Content = content;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Author = author;
        Operators = operators;
        CreateBy = createBy;
        UpdateBy = createBy;
    }

    /// <summary>
    ///     The constructor with all properties.
    /// </summary>
    /// <remarks>
    ///     THIS CONSTRUCTOR IS INTENDED TO BE USED BY THE UNIT TEST ONLY.
    /// </remarks>
    /// <param name="id">The ID.</param>
    /// <param name="content">The content.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="author">The author of the operation.</param>
    /// <param name="createBy">The creator of the operation.</param>
    /// <param name="operators">The operators in the operation.</param>
    public CopilotOperation(long id,
        string content,
        string stageName,
        string minimumRequired,
        string title,
        string details,
        CopilotUser author,
        Guid createBy,
        List<string> operators)
        : this(content, stageName, minimumRequired, title, details, author, createBy, operators)
    {
        Id = id;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotOperation() { }
#pragma warning restore CS8618

    // WARNING:
    // YOU SHOULD NEVER EXPOSE SETTER TO PUBLIC SCOPE.
    // YOU SHOULD NEVER EXPOSE DEFAULT CONSTRUCTOR TO PUBLIC SCOPE.
    // YOU SHOULD ONLY USE A DOMAIN METHOD TO UPDATE PROPERTIES.
    // YOU SHOULD CALL DELETE METHOD BEFORE YOU ACTUALLY DELETE IT.

    /// <summary>
    ///     Copilot operation id (int).
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public long Id { get; private set; }

    /// <summary>
    ///     Copilot operation content (JSON).
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    ///     View counts.
    /// </summary>
    public int ViewCounts { get; private set; }

    /// <summary>
    ///     Favorite counts.
    /// </summary>
    public int Favorites { get; private set; }

    // Extract from Content

    /// <summary>
    ///     The stage name.
    /// </summary>
    public string StageName { get; private set; }

    /// <summary>
    ///     The minimum required version of MAA.
    /// </summary>
    public string MinimumRequired { get; private set; }

    /// <summary>
    ///     The title of the operation.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    ///     The operators in the operation.
    /// </summary>
    public List<string> Operators { get; private set; }

    /// <summary>
    ///     The detail of the operation.
    /// </summary>
    public string Details { get; private set; }

    /// <summary>
    ///     The uploader of the operation.
    /// </summary>
    public CopilotUser Author { get; private set; }

    /// <summary>
    ///     M2M relation. DO NOT INCLUDE QUERY THIS.
    /// </summary>
    public List<CopilotUserFavorite> FavoriteBy { get; private set; } = new();

    /// <summary>
    ///     Increases download count by 1, and updates last updated time.
    /// </summary>
    /// <remarks>This API is reachable anonymously, so the update will not be recorded.</remarks>
    public void AddViewCount()
    {
        ViewCounts++;
        UpdateAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Add a favorite count, and updates last updated time.
    /// </summary>
    public void AddFavorites(Guid @operator)
    {
        Favorites++;
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
    }

    /// <summary>
    ///     Remove a favorite count, and updates last updated time.
    /// </summary>
    public void RemoveFavorites(Guid @operator)
    {
        Favorites--;
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
    }
}
