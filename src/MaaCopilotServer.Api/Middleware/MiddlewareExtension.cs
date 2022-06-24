// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Api.Middleware;

/// <summary>
///     The middleware extension.
/// </summary>
public static class MiddlewareExtension
{
    /// <summary>
    ///     Uses request culture.
    /// </summary>
    /// <param name="app">The app</param>
    /// <returns>The app with the middleware mounted.</returns>
    public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestCultureMiddleware>();
        return app;
    }

    public static IApplicationBuilder UseApmTransaction(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApmTransactionMiddleware>();
        return app;
    }
}
