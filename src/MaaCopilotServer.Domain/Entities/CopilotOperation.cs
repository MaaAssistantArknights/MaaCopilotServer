// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Maa Copilot operation entity.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CopilotOperation : EditableEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperation" />.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="author">The author of the operation.</param>
    /// <param name="createBy">The creator of the operation.</param>
    /// <param name="level">The level this operation is made for.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    /// <param name="difficulty">The difficulty of the operation.</param>
    public CopilotOperation(string content, string minimumRequired, string title, string details,
        CopilotUser author, Guid createBy, ArkLevelData level, IEnumerable<string> operators, IEnumerable<string> groups, DifficultyType difficulty = DifficultyType.Unknown)
    {
        Content = content;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Author = author;
        ArkLevel = level;
        Operators = operators.ToList();
        Groups = groups.ToList();
        CreateBy = createBy;
        UpdateBy = createBy;
        Difficulty = difficulty;
    }

    /// <summary>
    ///     The constructor with all properties.
    /// </summary>
    /// <remarks>
    ///     THIS CONSTRUCTOR IS INTENDED TO BE USED BY THE UNIT TEST ONLY.
    /// </remarks>
    /// <param name="id">The ID.</param>
    /// <param name="content">The content.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="author">The author of the operation.</param>
    /// <param name="createBy">The creator of the operation.</param>
    /// <param name="level">The level this operation is made for.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    /// <param name="difficulty">The difficulty of the operation.</param>
    public CopilotOperation(
        long id,
        string content,
        string minimumRequired,
        string title,
        string details,
        CopilotUser author,
        Guid createBy,
        ArkLevelData level,
        IEnumerable<string> operators,
        IEnumerable<string> groups,
        DifficultyType difficulty = DifficultyType.Unknown)
        : this(content, minimumRequired, title, details, author, createBy, level, operators, groups, difficulty)
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
    ///     Like counts.
    /// </summary>
    public int LikeCount { get; private set; }

    /// <summary>
    ///     Dislike counts.
    /// </summary>
    public int DislikeCount { get; private set; }

    // Extract from Content

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
    ///     The level this operation is made for.
    /// </summary>
    public ArkLevelData ArkLevel { get; private set; }

    // Auto calculated properties

    /// <summary>
    ///     The hot score of the operation.
    /// </summary>
    public long HotScore { get; private set; } = 0;

    /// <summary>
    ///     Current rating level.
    /// </summary>
    public RatingLevel RatingLevel { get; private set; } = RatingLevel.Mixed;

    /// <summary>
    ///     Current rating ratio.
    /// </summary>
    public double RatingRatio { get; private set; }

    /// <summary>
    ///     Is not enough rating.
    /// </summary>
    public bool IsNotEnoughRating { get; private set; } = true;

    /// <summary>
    /// The difficulty.
    /// </summary>
    public DifficultyType Difficulty { get; private set; } = DifficultyType.Unknown;

    /// <summary>
    ///     Increases view count by 1, and updates last updated time.
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
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    /// <param name="operator">The one who call this method.</param>
    /// <param name="difficulty">The difficulty of the operation.</param>
    public void UpdateOperation(string content, string minimumRequired, string title, string details,
        IEnumerable<string> operators, IEnumerable<string> groups, Guid @operator, DifficultyType difficulty = DifficultyType.Unknown)
    {
        Content = content;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Operators = operators.ToList();
        Groups = groups.ToList();

        if (difficulty != DifficultyType.Unknown)
        {
            Difficulty = difficulty;
        }

        Update(@operator);
    }

    /// <summary>
    ///     Add a like count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void AddLike(Guid @operator)
    {
        LikeCount++;
        UpdateRatingLevel();
        Update(@operator);
    }

    /// <summary>
    ///     Add a dislike count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void AddDislike(Guid @operator)
    {
        DislikeCount++;
        UpdateRatingLevel();
        Update(@operator);
    }

    /// <summary>
    ///     Remove a like count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void RemoveLike(Guid @operator)
    {
        LikeCount--;
        UpdateRatingLevel();
        Update(@operator);
    }

    /// <summary>
    ///     Remove a dislike count, and updates last updated time.
    /// </summary>
    /// <param name="operator">The operator id.</param>
    public void RemoveDislike(Guid @operator)
    {
        DislikeCount--;
        UpdateRatingLevel();
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

    /// <summary>
    ///     Calculate current like to all ratio and set rating level.
    /// </summary>
    private void UpdateRatingLevel()
    {
        var total = this.LikeCount + this.DislikeCount;
        
        // TODO: Extract to Environment Variable
        if (total < 5)
        {
            this.IsNotEnoughRating = true;
            this.RatingLevel = RatingLevel.Mixed;
            return;
        }

        this.IsNotEnoughRating = false;
        this.RatingRatio = Math.Round(this.LikeCount / (double)total, 2);
        this.RatingLevel = this.RatingRatio switch
        {
            >= 0.9f => RatingLevel.OverwhelminglyPositive,
            >= 0.8f => RatingLevel.VeryPositive,
            >= 0.7f => RatingLevel.Positive,
            >= 0.6f => RatingLevel.MostlyPositive,
            >= 0.5f => RatingLevel.Mixed,
            >= 0.4f => RatingLevel.Negative,
            >= 0.3f => RatingLevel.MostlyNegative,
            >= 0.2f => RatingLevel.VeryNegative,
            _ => RatingLevel.OverwhelminglyNegative
        };
    }

    /// <summary>
    ///     Update the hot score.
    /// </summary>
    /// <param name="hotScore">The current hot score.</param>
    public void UpdateHotScore(long hotScore)
    {
        HotScore = hotScore;
    }
}
