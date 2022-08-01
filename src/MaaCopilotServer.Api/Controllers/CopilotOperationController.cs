// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application.Common.Models;
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
///     The controller of copilot operations under "copilot" endpoint.
/// Include operations related to copilot operations.
/// </summary>
/// <response code="400">A bad request, most cases are invalid request parameters.</response>
/// <response code="401">An unauthorized request, you need to login and set Authorization header at first.</response>
/// <response code="403">A forbidden request, you do not have permission to perform the operation.</response>
/// <response code="404">Some thing not found.</response>
/// <response code="500">Some server errors happens.</response>
[ApiController]
[Route("copilot")]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status500InternalServerError)]
public class CopilotOperationController : MaaControllerBase
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperationController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public CopilotOperationController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     Upload a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The operation is successfully uploaded.</response>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(MaaApiResponseModel<CreateCopilotOperationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateCopilotOperation([FromBody] CreateCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Delete a copilot operation.
    /// </summary>
    /// <param name="command">The request boy.</param>
    /// <response code="200">The operation was successfully deleted.</response>
    [HttpPost("delete")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCopilotOperation([FromBody] DeleteCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Get a copilot operation by its id.
    /// </summary>
    /// <param name="id">The operation id.</param>
    /// <param name="server">The server language. Could be (ignore case) Chinese (Default), English, Japanese, Korean.</param>
    /// <response code="200">The operation JSON and related metadata.</response>
    [HttpGet("get/{id}")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCopilotOperationQueryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCopilotOperation(string id, [FromQuery(Name = "language")] string? language)
    {
        var request = new GetCopilotOperationQuery { Id = id, Language = language };
        return await GetResponse(request);
    }

    /// <summary>
    ///     Query copilot operations.
    /// </summary>
    /// <param name="query">The request body.</param>
    /// <response code="200">A list of query results with operation metadata.</response>
    [HttpGet("query")]
    [ProducesResponseType(typeof(MaaApiResponseModel<PaginationResult<QueryCopilotOperationsQueryDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> QueryCopilotOperation([FromQuery] QueryCopilotOperationsQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     Update a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The operation was successfully updated.</response>
    [HttpPost("update")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateCopilotOperation([FromBody] UpdateCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Rate a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The rating was successfully added to the operation.</response>
    [HttpPost("rating")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCopilotOperationQueryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RatingCopilotOperation([FromBody] RatingCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }
}
