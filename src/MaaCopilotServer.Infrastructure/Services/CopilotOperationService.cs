// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Resources;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Infrastructure.Services;

/// <summary>
///     The service for copilot operation related things.
/// </summary>
public class CopilotOperationService : ICopilotOperationService
{
    private readonly DomainString _domainString;

    /// <summary>
    ///     The minimum ID value. Other IDs should be calculated based on this value.
    /// </summary>
    // 不准改这个值!
    // DO NOT CHANGE THIS VALUE!
    // この値は変更しないでください!
    private const long MinimumId = 10000;

    private readonly Func<int, int, int, long> _hotScoreCalculator;

    public CopilotOperationService(
        IOptions<CopilotOperationOption> copilotOperationOption,
        DomainString domainString)
    {
        _domainString = domainString;
        var initialScore = copilotOperationOption.Value.InitialHotScore;
        var likeMultiplier = copilotOperationOption.Value.LikeMultiplier;
        var dislikeMultiplier = copilotOperationOption.Value.DislikeMultiplier;
        var viewMultiplier = copilotOperationOption.Value.ViewMultiplier;

        _hotScoreCalculator = (like, dislike, view) =>
        {
            var score = initialScore +
                        likeMultiplier * like +
                        dislikeMultiplier * dislike +
                        viewMultiplier * view;
            var scoreRounded = (long)Math.Round(score);
            var scoreNorm = Math.Max(0, scoreRounded);
            return scoreNorm;
        };
    }

    /// <inheritdoc />
    public string EncodeId(long plainId)
    {
        return (plainId + MinimumId).ToString();
    }

    /// <inheritdoc />
    public long? DecodeId(string encodedId)
    {
        var parsable = long.TryParse(encodedId, out var value);
        if (parsable is false)
        {
            return null;
        }

        if (value < MinimumId)
        {
            return null;
        }

        return value - MinimumId;
    }

    /// <inheritdoc />
    public long CalculateHotScore(CopilotOperation operation)
    {
        return this.CalculateHotScore(operation.LikeCount, operation.DislikeCount, operation.ViewCounts);
    }

    /// <inheritdoc />
    public long CalculateHotScore(int likes, int dislikes, int views)
    {
        return _hotScoreCalculator.Invoke(likes, dislikes, views);
    }

    /// <inheritdoc />
    public string GetRatingLevelString(RatingLevel ratingLevel)
    {
        var value = ratingLevel switch
        {
            RatingLevel.OverwhelminglyPositive => _domainString.OverwhelminglyPositive,
            RatingLevel.VeryPositive => _domainString.VeryPositive,
            RatingLevel.Positive => _domainString.Positive,
            RatingLevel.MostlyPositive => _domainString.MostlyPositive,
            RatingLevel.Mixed => _domainString.Mixed,
            RatingLevel.MostlyNegative => _domainString.MostlyNegative,
            RatingLevel.Negative => _domainString.Negative,
            RatingLevel.VeryNegative => _domainString.VeryNegative,
            RatingLevel.OverwhelminglyNegative => _domainString.OverwhelminglyNegative,
            _ => null
        };

        if (value is null)
        {
            throw new ArgumentOutOfRangeException(nameof(ratingLevel), ratingLevel, null);
        }

        return value;
    }
}
