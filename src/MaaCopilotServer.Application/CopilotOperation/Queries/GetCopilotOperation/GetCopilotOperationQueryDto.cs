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
