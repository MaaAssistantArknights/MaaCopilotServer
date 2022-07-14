// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Test.TestEntities;

/// <summary>
/// The factory class of <see cref="CopilotOperationRating"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotOperationRatingFactory : ITestEntityFactory<CopilotOperationRating>
{
    /// <summary>
    ///     The operation id.
    /// </summary>
    public Guid OperationId { get; set; } = Guid.Empty;

    /// <summary>
    ///     The user id.
    /// </summary>
    public Guid UserId { get; set; } = Guid.Empty;

    /// <summary>
    ///     The rating.
    /// </summary>
    public OperationRatingType RatingType { get; set; } = OperationRatingType.None;

    /// <summary>
    ///     Creator GUID
    /// </summary>
    public Guid CreateBy { get; set; } = Guid.Empty;

    /// <inheritdoc/>
    public CopilotOperationRating Build()
    {
        return new CopilotOperationRating(OperationId, UserId, RatingType);
    }
}
