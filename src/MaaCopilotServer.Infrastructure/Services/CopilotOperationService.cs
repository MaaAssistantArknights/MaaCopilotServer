// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
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
    public long CalculateHotScore(CopilotOperation operation)
    {
        return this.CalculateHotScore(operation.LikeCount, operation.DislikeCount, operation.ViewCounts);
    }

    /// <inheritdoc />
    public long CalculateHotScore(int likes, int dislikes, int views)
    {
        return _hotScoreCalculator.Invoke(likes, dislikes, views);
    }
}
