// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Models;

/// <summary>
///     A DTO style class for <see cref="MaaCopilotOperationGroupStore"/>.
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
