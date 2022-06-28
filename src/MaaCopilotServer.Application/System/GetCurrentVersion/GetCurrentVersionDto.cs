// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.System.GetCurrentVersion;

/// <summary>
///     The response to the <see cref="GetCurrentVersionCommand"/>.
/// </summary>
public record GetCurrentVersionDto
{
    public GetCurrentVersionDto(string version, string time)
    {
        Version = version;
        Time = time;
    }

#pragma warning disable CS8618
    public GetCurrentVersionDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The version of the server.
    /// </summary>
    [Required]
    [JsonPropertyName("version")]
    public string Version { get; set; }

    /// <summary>
    ///     The current time of the server.
    /// </summary>
    [Required]
    [JsonPropertyName("time")]
    public string Time { get; set; }
}
