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
///     The controller of copilot operations under <c>/copilot</c> endpoint,
///     including operations related to copilot operations.
/// </summary>
/// <remarks>
/// Response codes:
/// <list type="bullet">
///     <item>
///         <term>400</term>
///         <description>A bad request. Most cases are invalid request parameters.</description>
///     </item>
///     <item>
///         <term>401</term>
///         <description>An unauthorized request. You need to login and set Authorization header at first.</description>
///     </item>
///     <item>
///         <term>403</term>
///         <description>A forbidden request. You do not have permission to perform the operation.</description>
///     </item>
///     <item>
///         <term>404</term>
///         <description>Something is not found.</description>
///     </item>
///     <item>
///         <term>500</term>
///         <description>Some server errors happens.</description>
///     </item>
/// </list>
/// </remarks>
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
    ///     Uploads a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The operation has been uploaded successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The JSON format is incorrect.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(MaaApiResponseModel<CreateCopilotOperationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateCopilotOperation([FromBody] CreateCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Deletes a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The operation has been successfully deleted.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>403</term>
    ///         <description>
    ///             The user is not allowed to delete this operation.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>
    ///             The operation specified is not found.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("delete")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCopilotOperation([FromBody] DeleteCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Gets a copilot operation by its id.
    /// </summary>
    /// <param name="id">The operation id.</param>
    /// <param name="language">
    ///     The server language.
    ///     
    ///     <para>List of available languages (case-insensitive):</para>
    ///     <list type="bullet">
    ///         <item>Chinese (Default)</item>
    ///         <item>English</item>
    ///         <item>Japanese</item>
    ///         <item>Korean</item>
    ///     </list>
    /// </param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The operation JSON and related metadata.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>
    ///             The operation specified is not found.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("get/{id}")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCopilotOperationQueryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCopilotOperation(string id, [FromQuery(Name = "language")] string? language)
    {
        var request = new GetCopilotOperationQuery { Id = id, Language = language };
        return await GetResponse(request);
    }

    /// <summary>
    ///     Queries copilot operations.
    /// </summary>
    /// <param name="query">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             A list of query results with operation metadata.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The uploader ID is set to <c>me</c>, but the requester has not logged in.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("query")]
    [ProducesResponseType(typeof(MaaApiResponseModel<PaginationResult<QueryCopilotOperationsQueryDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> QueryCopilotOperation([FromQuery] QueryCopilotOperationsQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     Updates a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The operation has been successfully updated.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The JSON format is incorrect, or the stage is inconsistent.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>403</term>
    ///         <description>
    ///             The user is not allowed to update this operation.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>
    ///             The operation specified is not found.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("update")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateCopilotOperation([FromBody] UpdateCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Rates a copilot operation.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The rating has been successfully added to the operation.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>
    ///             The operation specified is not found.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("rating")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCopilotOperationQueryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RatingCopilotOperation([FromBody] RatingCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }
}
