// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text;
using System.Text.Json;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace MaaCopilotServer.Api.Formatter;

public class MaaResponseFormatter : TextOutputFormatter
{
    public MaaResponseFormatter()
    {
        SupportedMediaTypes.Add("application/json");
        SupportedEncodings.Add(Encoding.UTF8);
    }

    protected override bool CanWriteType(Type? _)
    {
        return true;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var httpContext = context.HttpContext;
        var serviceProvider = httpContext.RequestServices;

        var currentUser = serviceProvider.GetService<ICurrentUserService>();

        var traceId = currentUser!.GetTrackingId();

        httpContext.Response.StatusCode = StatusCodes.Status200OK;

        if (context.Object is MaaApiResponse maaApiResponse)
        {
            maaApiResponse.TraceId = traceId;
            await httpContext.Response.WriteAsJsonAsync(maaApiResponse);
        }
        else
        {
            var unknownResponse = MaaApiResponseHelper.InternalError("Unknown response type");
            unknownResponse.TraceId = traceId;
            await httpContext.Response.WriteAsJsonAsync(unknownResponse);
        }
    }
}
