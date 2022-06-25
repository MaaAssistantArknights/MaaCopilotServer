// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// The JSON request content of creating copilot operation.
/// </summary>
public record CreateCopilotOperationContent
{
    /// <summary>
    /// The <c>stage_name</c> field.
    /// </summary>
    [JsonPropertyName("stage_name")]
    public string? StageName { get; set; }

    /// <summary>
    /// The <c>minimum_required</c> field.
    /// </summary>
    [JsonPropertyName("minimum_required")]
    public string? MinimumRequired { get; set; }

    /// <summary>
    /// The <c>doc</c> field.
    /// </summary>
    [JsonPropertyName("doc")]
    public Doc? Doc { get; set; }

    /// <summary>
    /// The <c>opers</c> field.
    /// </summary>
    [JsonPropertyName("opers")]
    public Operator[]? Operators { get; set; }
}


/// <summary>
/// The JSON content of <c>doc</c>.
/// </summary>
public record Doc
{
    /// <summary>
    /// The <c>title</c> field.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The <c>details</c> field.
    /// </summary>
    [JsonPropertyName("details")]
    public string? Details { get; set; }
}

/// <summary>
/// The JSON content of <c>operator</c>.
/// </summary>
public record Operator
{
    /// <summary>
    /// The <c>name</c> field.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The <c>skill</c> field.
    /// </summary>
    [JsonPropertyName("skill")]
    public int? Skill { get; set; }
}
