// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Models;

public class MaaApiResponse
{
    private MaaApiResponse(int statusCode, string message, string traceId, object? data)
    {
        StatusCode = statusCode;
        Message = message;
        TraceId = traceId;
        Data = data;
    }

    [JsonPropertyName("status_code")] public int StatusCode { get; }
    [JsonPropertyName("message")] public string Message { get; }
    [JsonPropertyName("trace_id")] public string TraceId { get; }
    [JsonPropertyName("data")] public object? Data { get; }

    public static MaaApiResponse Ok(object? obj, string id)
    {
        return new MaaApiResponse(200, "OK", id, obj);
    }

    public static MaaApiResponse Unauthorized(string id, string? message = null)
    {
        return new MaaApiResponse(401, message ?? "Unauthorized", id, null);
    }

    public static MaaApiResponse Forbidden(string id, string? message = null)
    {
        return new MaaApiResponse(403, message ?? "Forbidden. Permission Denied", id, null);
    }

    public static MaaApiResponse BadRequest(string id, string? message = null)
    {
        return new MaaApiResponse(400, message ?? "Bad Request", id, null);
    }

    public static MaaApiResponse NotFound(string resourceName, string id)
    {
        return new MaaApiResponse(404, $"{resourceName} Not Found", id, null);
    }

    public static MaaApiResponse InternalError(string id)
    {
        return new MaaApiResponse(500, "Internal Server Error", id, null);
    }
}
