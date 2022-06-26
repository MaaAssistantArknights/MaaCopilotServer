// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;
using MaaCopilotServer.Application.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Test.TestHelpers;

/// <summary>
/// The helper class for testing controllers.
/// </summary>
public static class ControllerTestHelper
{
    /// <summary>
    ///     Tests an endpoint of the controller.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request, e.g. a class end with "Command".</typeparam>
    /// <typeparam name="TController">The type of the controller, which should be a derived class of <see cref="MaaControllerBase"/>.</typeparam>
    /// <param name="testRequest">The test request.</param>
    /// <param name="testResponse">The test response.</param>
    /// <param name="controllerBuilder">The function to build a new controller with the mock mediator.</param>
    /// <param name="controllerAction">The method to call the controller with request.</param>
    public static void TestControllerEndpoint<TRequest, TController>(
        TRequest testRequest,
        object? testResponse,
        Func<IMediator, TController> controllerBuilder,
        Func<TController, TRequest, Task<ActionResult>> controllerAction)
        where TController : MaaControllerBase
    {
        var testResponseData = MaaApiResponseHelper.Ok(testResponse);
        var mediator = new Mock<IMediator>();
        mediator.Setup(x => x.Send(It.IsAny<It.IsAnyType>(), It.IsAny<CancellationToken>()).Result)
                .Returns(testResponseData);

        var controller = controllerBuilder(mediator.Object);

        var actualResponse = controllerAction(controller, testRequest).GetAwaiter().GetResult();
        actualResponse.Should().NotBeNull();
        actualResponse.Should().BeEquivalentTo(new OkObjectResult(MaaApiResponseHelper.Ok(testResponse)));
    }
}
