// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using MaaCopilotServer.Resources;

namespace MaaCopilotServer.Api.Middleware;

/// <summary>
/// The middleware of request culture.
/// </summary>
public class RequestCultureMiddleware
{
    /// <summary>
    /// The next request processor.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// The constructor of <see cref="RequestCultureMiddleware"/>.
    /// </summary>
    /// <param name="next">The next request processor.</param>
    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Processes the request.
    /// </summary>
    /// <param name="context">The context of the request.</param>
    /// <param name="validationErrorMessage">The error message of validation errors.</param>
    /// <param name="apiErrorMessage">The error message of API errors.</param>
    /// <returns>The result after processing.</returns>
    public async Task InvokeAsync(HttpContext context, ValidationErrorMessage validationErrorMessage, ApiErrorMessage apiErrorMessage)
    {
        var hasCulture = context.Request.Query.TryGetValue("culture", out var culture);
        var info = hasCulture ? new CultureInfo(culture) : new CultureInfo("zh-cn");

        validationErrorMessage.CultureInfo = info;
        apiErrorMessage.CultureInfo = info;

        await _next(context);
    }
}
