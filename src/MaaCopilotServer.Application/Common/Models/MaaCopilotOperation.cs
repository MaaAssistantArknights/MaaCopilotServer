// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Models;

/// <summary>
///     The JSON content of Maa Copilot Operation.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaaCopilotOperation
{
    /// <summary>
    ///     The <c>stage_name</c> field.
    /// </summary>
    [JsonPropertyName("stage_name")]
    public string? StageName { get; set; }

    /// <summary>
    ///     The <c>minimum_required</c> field.
    /// </summary>
    [JsonPropertyName("minimum_required")]
    public string? MinimumRequired { get; set; }

    /// <summary>
    ///     The <c>doc</c> field.
    /// </summary>
    [JsonPropertyName("doc")]
    public MaaCopilotOperationDoc? Doc { get; set; }

    /// <summary>
    ///     The <c>opers</c> field.
    /// </summary>
    [JsonPropertyName("opers")]
    public MaaCopilotOperationOperator[]? Operators { get; set; }

    /// <summary>
    ///     The <c>groups</c> field.
    /// </summary>
    [JsonPropertyName("groups")]
    public MaaCopilotOperationGroup[]? Groups { get; set; }
}


/// <summary>
///     The JSON content of <c>doc</c>.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaaCopilotOperationDoc
{
    /// <summary>
    ///     The <c>title</c> field.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    ///     The <c>details</c> field.
    /// </summary>
    [JsonPropertyName("details")]
    public string? Details { get; set; }
}

/// <summary>
///     The JSON content of <c>operator</c>.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaaCopilotOperationOperator
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

/// <summary>
///     The JSON content of <c>Group</c>.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaaCopilotOperationGroup
{
    /// <summary>
    ///     The <c>name</c> field.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     The <c>operators</c> field.
    /// </summary>
    [JsonPropertyName("opers")]
    public MaaCopilotOperationOperator[]? Operators { get; set; }
}

/// <summary>
///     A DTO style class for <see cref="MaaCopilotOperationGroup"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaaCopilotOperationGroupStore
{
    /// <summary>
    ///     The constructor of <see cref="MaaCopilotOperationGroupStore"/>.
    /// </summary>
    /// <param name="name">The group name.</param>
    /// <param name="operators">The operators list in domain level string format.</param>
    public MaaCopilotOperationGroupStore(string name, List<string> operators)
    {
        Name = name;
        Operators = operators;
    }

#pragma warning disable CS8618
    /// <summary>
    ///     The constructor of <see cref="MaaCopilotOperationGroupStore"/>.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public MaaCopilotOperationGroupStore() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The <c>name</c> field.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     The <c>operators</c> field but in formatted string mode.
    /// </summary>
    [JsonPropertyName("operators")]
    public List<string> Operators { get; set; }
}
