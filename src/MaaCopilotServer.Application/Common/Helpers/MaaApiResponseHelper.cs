// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

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
        /// <typeparam name="T">The response type.</typeparam>
        /// <returns>The response.</returns>
        public static MaaApiResponse<T> BadRequest<T>(string id, string? message)
        {
            return new MaaApiResponse<T>
            {
                StatusCode = 400,
                Message = message ?? "Bad Request",
                TraceId = id,
                Data = default,
            };
        }

        /// <summary>
        ///     Responds HTTP 403 Forbidden.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Forbidden. Permission Denied"</c> by default</param>
        /// <typeparam name="T">The response type.</typeparam>
        /// <returns>The response.</returns>
        public static MaaApiResponse<T> Forbidden<T>(string id, string? message)
        {
            return new MaaApiResponse<T>
            {
                StatusCode = 403,
                Message = message ?? "Forbidden. Permission Denied",
                TraceId = id,
                Data = default,
            };
        }

        /// <summary>
        ///     Responds HTTP 500 Internal Error.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Internal Server Error"</c> by default.</param>
        /// <typeparam name="T">The response type.</typeparam>
        /// <returns>The response.</returns>
        public static MaaApiResponse<T> InternalError<T>(string id, string? message)
        {
            return new MaaApiResponse<T>
            {
                StatusCode = 500,
                Message = message ?? "Internal Server Error",
                TraceId = id,
                Data = default,
            };
        }

        /// <summary>
        ///     Responds HTTP 404 Not Found.
        /// </summary>
        /// <param name="id">The tracking ID.</param>
        /// <param name="message">The message, <c>"Not Found"</c> by default.</param>
        /// <typeparam name="T">The response type.</typeparam>
        /// <returns>The response.</returns>
        public static MaaApiResponse<T> NotFound<T>(string id, string? message)
        {
            return new MaaApiResponse<T>
            {
                StatusCode = 404,
                Message = message ?? "Not Found",
                TraceId = id,
                Data = default,
            };
        }

        /// <summary>
        ///     Responds HTTP 200 OK.
        /// </summary>
        /// <param name="obj">The request body.</param>
        /// <param name="id">The tracking ID.</param>
        /// <typeparam name="T">The response type.</typeparam>
        /// <returns>The response.</returns>
        public static MaaApiResponse<T> Ok<T>(T? obj, string id)
        {
            return new MaaApiResponse<T>
            {
                StatusCode = 200,
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
        /// <typeparam name="T">The response type.</typeparam>
        /// <returns>The response.</returns>
        public static MaaApiResponse<T> Unauthorized<T>(string id, string? message)
        {
            return new MaaApiResponse<T>
            {
                StatusCode = 401,
                Message = message ?? "Unauthorized",
                TraceId = id,
                Data = default,
            };
        }
    }
}
