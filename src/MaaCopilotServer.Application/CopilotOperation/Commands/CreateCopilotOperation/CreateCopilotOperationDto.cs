// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
///     The response to a <see cref="CreateCopilotOperationCommand" />.
/// </summary>
[ExcludeFromCodeCoverage]
public record CreateCopilotOperationDto
{
    /// <summary>
    ///     The constructor of <see cref="CreateCopilotOperationDto"/>.
    /// </summary>
    /// <param name="id">The operation id.</param>
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
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
