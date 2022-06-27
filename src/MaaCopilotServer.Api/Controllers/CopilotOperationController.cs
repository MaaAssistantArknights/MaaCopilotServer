// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Commands.RatingCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Commands.UpdateCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller of copilot operations under <c>copilot</c> endpoint.
/// </summary>
[ApiController]
[Route("copilot")]
public class CopilotOperationController : MaaControllerBase
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperationController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public CopilotOperationController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     The handler of <c>upload</c> endpoint to create a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("upload")]
    public async Task<ActionResult> CreateCopilotOperation([FromBody] CreateCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>delete</c> endpoint to delete a copilot operation.
    /// </summary>
    /// <param name="command">The request boy.</param>
    /// <returns>The response.</returns>
    [HttpPost("delete")]
    public async Task<ActionResult> DeleteCopilotOperation([FromBody] DeleteCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>get/:id</c> endpoint to get a copilot operation.
    /// </summary>
    /// <param name="id">The path parameter <c>id</c>, which is the operation ID.</param>
    /// <returns>The response.</returns>
    [HttpGet("get/{id}")]
    public async Task<ActionResult> GetCopilotOperation(string id)
    {
        var request = new GetCopilotOperationQuery { Id = id };
        return await GetResponse(request);
    }

    /// <summary>
    ///     The handler of <c>query</c> endpoint to query a copilot operation.
    /// </summary>
    /// <param name="query">The query data.</param>
    /// <returns>The response.</returns>
    [HttpGet("query")]
    public async Task<ActionResult> QueryCopilotOperation([FromQuery] QueryCopilotOperationsQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     The handler of <c>update</c> endpoint to update a copilot operation.
    /// </summary>
    /// <param name="command">The update command.</param>
    /// <returns>The response.</returns>
    [HttpPost("update")]
    public async Task<ActionResult> UpdateCopilotOperation([FromBody] UpdateCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>rating</c> endpoint to rate a copilot operation.
    /// </summary>
    /// <param name="command">The rating command.</param>
    /// <returns></returns>
    [HttpPost("rating")]
    public async Task<ActionResult> RatingCopilotOperation([FromBody] RatingCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }
}
