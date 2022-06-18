// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Models;

/// <summary>
///     The model of API response.
/// </summary>
public class MaaApiResponse
{
    /// <summary>
    ///     The constructor of <see cref="MaaApiResponse" />.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    /// <param name="message">The message.</param>
    /// <param name="traceId">The tracking ID.</param>
    /// <param name="data">The request body.</param>
    private MaaApiResponse(int statusCode, string message, string traceId, object? data)
    {
        StatusCode = statusCode;
        Message = message;
        TraceId = traceId;
        Data = data;
    }

    /// <summary>
    ///     The status code.
    /// </summary>
    [JsonPropertyName("status_code")]
    public int StatusCode { get; }

    /// <summary>
    ///     The message.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; }

    /// <summary>
    ///     The tracking ID.
    /// </summary>
    [JsonPropertyName("trace_id")]
    public string TraceId { get; }

    /// <summary>
    ///     The request body.
    /// </summary>
    [JsonPropertyName("data")]
    public object? Data { get; }

    /// <summary>
    ///     Responds HTTP 200 OK.
    /// </summary>
    /// <param name="obj">The request body.</param>
    /// <param name="id">The tracking ID.</param>
    /// <returns>The response.</returns>
    public static MaaApiResponse Ok(object? obj, string id)
    {
        return new MaaApiResponse(200, "OK", id, obj);
    }

    /// <summary>
    ///     Responds HTTP 401 Unauthorized.
    /// </summary>
    /// <param name="id">The tracking ID.</param>
    /// <param name="message">The message, <c>"Unauthorized"</c> by default.</param>
    /// <returns>The response.</returns>
    public static MaaApiResponse Unauthorized(string id, string? message = null)
    {
        return new MaaApiResponse(401, message ?? "Unauthorized", id, null);
    }

    /// <summary>
    ///     Responds HTTP 403 Forbidden.
    /// </summary>
    /// <param name="id">The tracking ID.</param>
    /// <param name="message">The message, <c>"Forbidden. Permission Denied"</c> by default</param>
    /// <returns>The response.</returns>
    public static MaaApiResponse Forbidden(string id, string? message = null)
    {
        return new MaaApiResponse(403, message ?? "Forbidden. Permission Denied", id, null);
    }

    /// <summary>
    ///     Responds HTTP 400 Bad Request.
    /// </summary>
    /// <param name="id">The tracking ID.</param>
    /// <param name="message">The message, <c>"Bad Request"</c> by default.</param>
    /// <returns>The response.</returns>
    public static MaaApiResponse BadRequest(string id, string? message = null)
    {
        return new MaaApiResponse(400, message ?? "Bad Request", id, null);
    }

    /// <summary>
    ///     Responds HTTP 404 Not Found.
    /// </summary>
    /// <param name="id">The tracking ID.</param>
    /// <param name="message">The message, <c>"Not Found"</c> by default.</param>
    /// <returns>The response.</returns>
    public static MaaApiResponse NotFound(string id, string? message = null)
    {
        return new MaaApiResponse(404, message ?? "Not Found", id, null);
    }

    /// <summary>
    ///     Responds HTTP 500 Internal Error.
    /// </summary>
    /// <param name="id">The tracking ID.</param>
    /// <param name="message">The message, <c>"Internal Server Error"</c> by default.</param>
    /// <returns>The response.</returns>
    public static MaaApiResponse InternalError(string id, string? message = null)
    {
        return new MaaApiResponse(500, message ?? "Internal Server Error", id, null);
    }
}
