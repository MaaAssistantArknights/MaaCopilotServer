// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Constants;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Resources;

namespace MaaCopilotServer.Api.Middleware;

public class SystemStatusMiddleware
{
    private readonly RequestDelegate _next;

    public SystemStatusMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApiErrorMessage apiErrorMessage, ICurrentUserService currentUserService)
    {
        if (SystemStatus.IsOk)
        {
            await _next(context);
            return;
        }

        var response = MaaApiResponseHelper.BadRequest(apiErrorMessage.ServerNotReady);
        response.TraceId = currentUserService.GetTrackingId();

        context.Response.StatusCode = 200;
        await context.Response.WriteAsJsonAsync(response);
    }
}
