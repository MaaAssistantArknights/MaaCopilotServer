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
    ///     Encodes an ID.
    /// </summary>
    /// <param name="plainId">The ID of <see cref="long" /> type.</param>
    /// <returns>The ID of <see cref="string" /> type</returns>
    string EncodeId(long plainId);

    /// <summary>
    ///     Decodes an ID.
    /// </summary>
    /// <param name="encodedId">The ID of <see cref="string" /> type.</param>
    /// <returns>The ID of <see cref="long" /> type if it is valid, otherwise <c>null</c>.</returns>
    long? DecodeId(string encodedId);

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
