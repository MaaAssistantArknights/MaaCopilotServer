// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
///     The response of the <see cref="GetCopilotOperationQuery"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class GetCopilotOperationQueryDto : QueryCopilotOperationsQueryDto
{
    /// <summary>
    ///     The constructor of <see cref="GetCopilotOperationQueryDto" />.
    /// </summary>
    /// <param name="id">The operation ID.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="uploadTime">The time when the operation was uploaded.</param>
    /// <param name="uploader">The name of the uploader.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="detail">The detail of the operation.</param>
    /// <param name="viewCounts">The view counts of the operation.</param>
    /// <param name="ratingRatio">The like to all rating ratio.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    /// <param name="content">The JSON content of the operation.</param>
    /// <param name="ratingType">The rating type by current user.</param>
    public GetCopilotOperationQueryDto(
        string id,
        string stageName,
        string minimumRequired,
        string uploadTime,
        string uploader,
        string title,
        string detail,
        int viewCounts,
        float ratingRatio,
        IEnumerable<string> operators,
        IEnumerable<MaaCopilotOperationGroupStore> groups,
        string content,
        OperationRatingType? ratingType)
        : base(id, stageName, minimumRequired, uploadTime, uploader,title,
            detail, viewCounts, ratingRatio, operators, groups, ratingType)
    {
        Content = content;
    }

#pragma warning disable CS8618
    public GetCopilotOperationQueryDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The operation JSON content.
    /// </summary>
    [Required]
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
