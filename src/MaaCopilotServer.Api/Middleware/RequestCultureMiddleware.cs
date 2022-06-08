// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using MaaCopilotServer.Resources;

namespace MaaCopilotServer.Api.Middleware;

public class RequestCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ValidationErrorMessage validationErrorMessage, ApiErrorMessage apiErrorMessage)
    {
        var hasCulture = context.Request.Query.TryGetValue("culture", out var culture);
        var info = hasCulture ? new CultureInfo(culture) : new CultureInfo("zh-cn");

        validationErrorMessage.CultureInfo = info;
        apiErrorMessage.CultureInfo = info;

        await _next(context);
    }
}
