// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

public abstract class MaaControllerBase : ControllerBase
{
    protected readonly IMediator _mediator;

    protected MaaControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<ActionResult> GetResponse(object request)
    {
        try
        {
            dynamic response = (await _mediator.Send(request))!;
            return response;
        }
        catch (PipelineException ex)
        {
            return ex.Result;
        }
    }
}
