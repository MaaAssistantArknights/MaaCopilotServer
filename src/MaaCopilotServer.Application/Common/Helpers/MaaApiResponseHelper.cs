// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Common.Helpers
{
    /// <summary>
    ///     The helper class of <see cref="MaaApiResponse"/>.
    /// </summary>
    public static class MaaApiResponseHelper
    {

        /// <summary>
        ///     Responds HTTP 400 Bad Request.
        /// </summary>
        /// <param name="message">The message, <c>"Bad Request"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse BadRequest(string? message = null)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message ?? "Bad Request",
                TraceId = string.Empty,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 403 Forbidden.
        /// </summary>
        /// <param name="message">The message, <c>"Forbidden. Permission Denied"</c> by default</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse Forbidden(string? message = null)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = message ?? "Forbidden. Permission Denied",
                TraceId = string.Empty,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 500 Internal Error.
        /// </summary>
        /// <param name="message">The message, <c>"Internal Server Error"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse InternalError(string? message = null)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = message ?? "Internal Server Error",
                TraceId = string.Empty,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 404 Not Found.
        /// </summary>
        /// <param name="message">The message, <c>"Not Found"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse NotFound(string? message = null)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = message ?? "Not Found",
                TraceId = string.Empty,
                Data = null,
            };
        }

        /// <summary>
        ///     Responds HTTP 200 OK.
        /// </summary>
        /// <param name="obj">The request body.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse Ok(object? obj = null)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "OK",
                TraceId = string.Empty,
                Data = obj,
            };
        }

        /// <summary>
        ///     Responds HTTP 401 Unauthorized.
        /// </summary>
        /// <param name="message">The message, <c>"Unauthorized"</c> by default.</param>
        /// <returns>The response.</returns>
        public static MaaApiResponse Unauthorized(string? message = null)
        {
            return new MaaApiResponse
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = message ?? "Unauthorized",
                TraceId = string.Empty,
                Data = null,
            };
        }
    }
}
