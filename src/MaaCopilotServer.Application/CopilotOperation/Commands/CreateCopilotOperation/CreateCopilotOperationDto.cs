// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
///     The DTO of creating operation.
/// </summary>
public class CreateCopilotOperationDto
{
    /// <summary>
    ///     The constructor of <see cref="CreateCopilotOperationDto" />.
    /// </summary>
    /// <param name="id">The operation ID.</param>
    public CreateCopilotOperationDto(string id)
    {
        Id = id;
    }

#pragma warning disable CS8618
    public CreateCopilotOperationDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The operation ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
