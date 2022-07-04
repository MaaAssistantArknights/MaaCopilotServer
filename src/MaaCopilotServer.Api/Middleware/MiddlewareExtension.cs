// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;

namespace MaaCopilotServer.Api.Middleware;

/// <summary>
///     The middleware extension.
/// </summary>
[ExcludeFromCodeCoverage]
public static class MiddlewareExtension
{
    /// <summary>
    ///     Use request culture middleware.
    /// </summary>
    /// <param name="app">The app pipeline.</param>
    /// <returns>The app pipeline with the middleware mounted.</returns>
    public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestCultureMiddleware>();
        return app;
    }

    /// <summary>
    ///     Use apm transaction middleware.
    /// </summary>
    /// <param name="app">The app pipeline.</param>
    /// <returns>The app pipeline with the middleware mounted.</returns>
    public static IApplicationBuilder UseApmTransaction(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApmTransactionMiddleware>();
        return app;
    }
}
