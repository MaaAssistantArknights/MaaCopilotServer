// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics;
using Elastic.Apm.Api;
using Microsoft.AspNetCore.Http.Extensions;

namespace MaaCopilotServer.Api.Middleware;

/// <summary>
/// Middleware to handle APM transactions.
/// </summary>
public class ApmTransactionMiddleware
{
    private readonly RequestDelegate _next;

    public ApmTransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<ApmTransactionMiddleware> logger)
    {
        var routerValues = context.Request.RouteValues;
        var routerName = routerValues.ContainsKey("controller") ? routerValues["controller"]!.ToString() : "UnknownRouter";
        var actionName = routerValues.ContainsKey("action") ? routerValues["action"]!.ToString() : "UnknownAction";

        var otherKeys = routerValues.Keys
            .SkipWhile(x => x.ToString() == "controller" || x.ToString() == "action")
            .Select(x => $"{{{x}}}")
            .ToArray();
        var otherKeyString = string.Join(" ", otherKeys);

        var name = $"{context.Request.Method} {routerName}/{actionName} {otherKeyString}".Trim();

        var url = new Url
        {
            Full = context.Request.GetDisplayUrl(),
            HostName = context.Request.Host.Value,
            PathName = context.Request.Path,
            Protocol = context.Request.Protocol,
            Search = context.Request.QueryString.ToString()
        };

        var transaction = Elastic.Apm.Agent.Tracer.StartTransaction(name, ApiConstants.TypeRequest);
        transaction.Context.Request = new Request(context.Request.Method, url)
        {
            Headers = context.Request.Headers
                .SkipWhile(x => x.Key == "Cookie")
                .ToDictionary(x => x.Key,
                    x => x.Value.ToString())
        };

        // Add APM trace id to HttpContext.
        context.Items.Add("ApmTraceId", transaction.TraceId);

        try
        {
            var sw = Stopwatch.StartNew();
            await _next(context);
            sw.Stop();

            var realStatusCode = 0;
            var isRealStatusCodeExist = context.Items.TryGetValue("StatusCode", out var statusCodeObj);
            if (isRealStatusCodeExist)
            {
                realStatusCode = statusCodeObj is null ? 0 : (int)statusCodeObj;
            }

            transaction.Context.Response = new Response
            {
                Finished = true,
                StatusCode = realStatusCode,
                Headers = context.Response.Headers
                    .ToDictionary(x => x.Key,
                        x => x.Value.ToString())
            };
        }
        catch (Exception ex)
        {
            transaction.Result = StatusCodes.Status500InternalServerError.ToString();
            transaction.Context.Response = new Response
            {
                Finished = false, StatusCode = StatusCodes.Status500InternalServerError
            };
            transaction.Result = ex.Message;
        }
        finally
        {
            transaction.End();
        }
    }
}
