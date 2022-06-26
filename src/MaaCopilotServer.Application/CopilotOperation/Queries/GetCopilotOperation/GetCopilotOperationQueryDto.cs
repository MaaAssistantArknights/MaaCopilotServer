// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
///     The DTO of querying operation.
/// </summary>
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
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    /// <param name="content">The JSON content of the operation.</param>
    public GetCopilotOperationQueryDto(
        string id,
        string stageName,
        string minimumRequired,
        string uploadTime,
        string uploader,
        string title,
        string detail,
        int viewCounts,
        IEnumerable<string> operators,
        IEnumerable<MaaCopilotOperationGroupStore> groups,
        string content)
        : base(id, stageName, minimumRequired, uploadTime, uploader, title, detail, viewCounts, operators, groups)
    {
        Content = content;
    }

#pragma warning disable CS8618
    public GetCopilotOperationQueryDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The content.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
