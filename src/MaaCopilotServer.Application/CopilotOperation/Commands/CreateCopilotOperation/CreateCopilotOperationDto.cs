// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
///     The DTO of creating operation.
/// </summary>
public record CreateCopilotOperationDto
{
    /// <summary>
    ///     The operation ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
