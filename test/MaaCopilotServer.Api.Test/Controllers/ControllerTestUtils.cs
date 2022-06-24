// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Test.Properties;

internal sealed class ControllerTestUtils
{
    /// <summary>
    ///     Tests an endpoint of the controller.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request, e.g. a class end with "Command".</typeparam>
    /// <typeparam name="TResponse">The type of the response, usually a DTO, or <see cref="GetCopilotUserDto" />, or even null.</typeparam>
    /// <param name="mediator">The mediator.</param>
    /// <param name="testRequest">The test request.</param>
    /// <param name="testResponse">The test response.</param>
    /// <param name="controllerAction">The method to call the controller.</param>
    /// <returns>N/A</returns>
    internal static async Task TestControllerEndpoint<TRequest, TResponse>(IMediator mediator, TRequest testRequest,
        TResponse testResponse, Func<TRequest, Task<ActionResult>> controllerAction)
    {
        var testResponseData = MaaApiResponseHelper.Ok<TResponse>(testResponse, string.Empty);
        mediator.Send(default).ReturnsForAnyArgs(testResponseData);

        var actualResponse = await controllerAction.Invoke(testRequest);
        actualResponse.Should().NotBeNull();
        actualResponse.Should().BeEquivalentTo(new OkObjectResult(testResponseData));
    }

    /// <summary>
    ///     Tests an endpoint of the controller with exception thrown.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request, e.g. a class end with "Command".</typeparam>
    /// <param name="mediator">The mediator.</param>
    /// <param name="testRequest">The test request.</param>
    /// <param name="testResponse">The test response.</param>
    /// <returns>N/A</returns>
    internal static async Task TestControllerEndpointWithException<TRequest>(IMediator mediator, TRequest testRequest,
        Func<TRequest, Task<ActionResult>> controllerAction)
    {
        var testResponseData = MaaApiResponseHelper.Ok<GetCopilotUserDto>(new GetCopilotUserDto(), string.Empty);
        mediator.Send(default).ThrowsForAnyArgs(new PipelineException(testResponseData));

        ActionResult? actualResponse = default;
        var action = async () => { actualResponse = await controllerAction.Invoke(testRequest); };
        await action.Should().NotThrowAsync(); // PipelineException should be catched.
        actualResponse.Should().NotBeNull();
        actualResponse.Should().BeEquivalentTo(new OkObjectResult(testResponseData));
    }
}
