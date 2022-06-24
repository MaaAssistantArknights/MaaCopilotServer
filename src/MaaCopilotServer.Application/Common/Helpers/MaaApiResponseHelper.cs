// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Common.Helpers
{
    /// <summary>
    ///     The helper class of <see cref="MaaApiResponse{T}"/>.
    /// </summary>
    public static class MaaApiResponseHelper
    {

        /// <summary>
        ///     Responds HTTP 400 Bad Request.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Bad Request"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse BadRequest(string id, string? message)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message ?? "Bad Request",
                TraceId = id,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 403 Forbidden.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Forbidden. Permission Denied"</c> by default</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse Forbidden(string id, string? message)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = message ?? "Forbidden. Permission Denied",
                TraceId = id,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 500 Internal Error.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Internal Server Error"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse InternalError(string id, string? message)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = message ?? "Internal Server Error",
                TraceId = id,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 404 Not Found.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Not Found"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse NotFound(string id, string? message)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = message ?? "Not Found",
                TraceId = id,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 200 OK.
        /// </summary>
        /// <param name="obj">The request body.</param>
        /// <param name="id">The tracking ID.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse Ok(object? obj, string id)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "OK",
                TraceId = id,
                Data = obj,
            };
        }

        /// <summary>
        ///     Responds HTTP 401 Unauthorized.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Unauthorized"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse Unauthorized(string id, string? message)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = message ?? "Unauthorized",
                TraceId = id,
                Data = null,
            };
        }
    }
}
