// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Api.Middleware;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestCultureMiddleware>();
        return app;
    }
}
