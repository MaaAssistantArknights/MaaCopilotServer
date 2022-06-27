// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     The entity to store user ratings.
/// </summary>
public class CopilotOperationRating : BaseEntity
{
    public CopilotOperationRating(Guid operationId, Guid userId, OperationRatingType ratingType)
    {
        OperationId = operationId;
        UserId = userId;
        RatingType = ratingType;

        CreateBy = userId;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotOperationRating() { }
#pragma warning restore CS8618

    // WARNING:
    // YOU SHOULD NEVER EXPOSE SETTER TO PUBLIC SCOPE.
    // YOU SHOULD NEVER EXPOSE DEFAULT CONSTRUCTOR TO PUBLIC SCOPE.
    // YOU SHOULD ONLY USE A DOMAIN METHOD TO UPDATE PROPERTIES.
    // YOU SHOULD CALL DELETE METHOD BEFORE YOU ACTUALLY DELETE IT.

    /// <summary>
    ///     The operation id.
    /// </summary>
    public Guid OperationId { get; private set; }

    /// <summary>
    ///     The user id.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    ///     The rating.
    /// </summary>
    public OperationRatingType RatingType { get; private set; }

    /// <summary>
    ///     Change the rating value.
    /// </summary>
    /// <param name="ratingType">Rating type.</param>
    public void ChangeRating(OperationRatingType ratingType)
    {
        RatingType = ratingType;
    }
}
