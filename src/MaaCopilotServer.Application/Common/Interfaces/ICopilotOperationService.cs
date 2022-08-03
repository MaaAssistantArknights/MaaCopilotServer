// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
///     The service for copilot operation related things.
/// </summary>
public interface ICopilotOperationService
{
    /// <summary>
    ///     Calculates the hot score of a operation.
    /// </summary>
    /// <param name="operation"></param>
    /// <returns></returns>
    long CalculateHotScore(Domain.Entities.CopilotOperation operation);

    /// <summary>
    ///     Calculates the hot score of a operation.
    /// </summary>
    /// <param name="likes">The like count.</param>
    /// <param name="dislikes">The dislike count.</param>
    /// <param name="views">The views count.</param>
    /// <returns></returns>
    long CalculateHotScore(int likes, int dislikes, int views);

    /// <summary>
    ///     Get rating level string, i18n.
    /// </summary>
    /// <param name="ratingLevel">Current rating level.</param>
    /// <returns>An i18n string.</returns>
    string GetRatingLevelString(RatingLevel ratingLevel);
}
