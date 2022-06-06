// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Api.Dtos;

public class DeleteCopilotOperationDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
