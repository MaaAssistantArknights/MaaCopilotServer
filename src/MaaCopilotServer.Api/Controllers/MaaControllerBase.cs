// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The base class of the controllers of MAA APIs.
/// </summary>
public abstract class MaaControllerBase : ControllerBase
{
    /// <summary>
    ///     The mediator.
    /// </summary>
    protected readonly IMediator _mediator;

    /// <summary>
    ///     The constructor of <see cref="MaaControllerBase" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    protected MaaControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Gets an API response of a request asynchronously.
    ///     It will send the request via mediator and waits for the response.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <returns>A task with the API response.</returns>
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
