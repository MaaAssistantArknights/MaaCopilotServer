// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Helper;

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
    /// <param name="groups">The groups in the operation.</param>
    public CopilotOperation(string content, string stageName, string minimumRequired, string title, string details,
        CopilotUser author, Guid createBy, IEnumerable<string> operators, IEnumerable<string> groups)
    {
        Content = content;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Author = author;
        Operators = operators.ToList();
        Groups = groups.ToList();
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
    /// <param name="groups">The groups in the operation.</param>
    public CopilotOperation(long id,
        string content,
        string stageName,
        string minimumRequired,
        string title,
        string details,
        CopilotUser author,
        Guid createBy,
        IEnumerable<string> operators,
        IEnumerable<string> groups)
        : this(content, stageName, minimumRequired, title, details, author, createBy, operators, groups)
    {
        Id = id;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    [ExcludeFromCodeCoverage]
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
    public int FavoriteCount { get; private set; }

    /// <summary>
    ///     Like counts.
    /// </summary>
    public int LikeCount { get; private set; }

    /// <summary>
    ///     Dislike counts.
    /// </summary>
    public int DislikeCount { get; private set; }

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
    ///     The groups in the operation.
    /// </summary>
    public List<string> Groups { get; private set; }

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
    public List<CopilotUserFavorite> Favorites { get; private set; } = new();

    // Auto calculated properties

    /// <summary>
    ///     The rating ratio of the operation.
    /// </summary>
    public float RatingRatio { get; private set; } = -1f;

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
    ///     Update this operation.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    /// <param name="operator">The one who call this method.</param>
    public void UpdateOperation(string content, string stageName, string minimumRequired, string title, string details,
        IEnumerable<string> operators, IEnumerable<string> groups, Guid @operator)
    {
        Content = content;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Operators = operators.ToList();
        Groups = groups.ToList();

        Update(@operator);
    }

    /// <summary>
    ///     Add a favorite count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void AddFavorites(Guid @operator)
    {
        FavoriteCount++;
        Update(@operator);
    }

    /// <summary>
    ///     Remove a favorite count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void RemoveFavorites(Guid @operator)
    {
        FavoriteCount--;
        Update(@operator);
    }

    /// <summary>
    ///     Add a like count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void AddLike(Guid @operator)
    {
        LikeCount++;
        RatingRatio = MathHelper.CalculateRatio(this.LikeCount, this.DislikeCount);
        Update(@operator);
    }

    /// <summary>
    ///     Add a dislike count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void AddDislike(Guid @operator)
    {
        DislikeCount++;
        RatingRatio = MathHelper.CalculateRatio(this.LikeCount, this.DislikeCount);
        Update(@operator);
    }

    /// <summary>
    ///     Remove a like count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void RemoveLike(Guid @operator)
    {
        LikeCount--;
        RatingRatio = MathHelper.CalculateRatio(this.LikeCount, this.DislikeCount);
        Update(@operator);
    }

    /// <summary>
    ///     Remove a dislike count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void RemoveDislike(Guid @operator)
    {
        DislikeCount--;
        RatingRatio = MathHelper.CalculateRatio(this.LikeCount, this.DislikeCount);
        Update(@operator);
    }

    /// <summary>
    ///     Update the last updated time and set operator.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    private void Update(Guid @operator)
    {
        UpdateAt = DateTimeOffset.UtcNow;
        UpdateBy = @operator;
    }
}
