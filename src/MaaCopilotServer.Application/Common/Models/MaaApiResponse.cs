// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Models;

/// <summary>
///     The model of API response.
/// </summary>
[ExcludeFromCodeCoverage]
public record MaaApiResponse
{
    /// <summary>
    ///     The status code.
    /// </summary>
    [Required]
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    /// <summary>
    ///     The message.
    /// </summary>
    [Required]
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     The tracking ID.
    /// </summary>
    [Required]
    [JsonPropertyName("trace_id")]
    public string TraceId { get; set; } = string.Empty;

    /// <summary>
    ///     The response body.
    /// </summary>
    [Required]
    [JsonPropertyName("data")]
    public object? Data { get; set; }
}
