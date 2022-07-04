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
    /// <typeparam name="TController">
    ///     The type of the controller, which should be a derived class of <see cref="MaaControllerBase"/>.
    /// </typeparam>
    /// <param name="testMethod">The function to call a controller method.</param>
    public static void Test<TController>(Func<TController, Task<ActionResult>> testMethod)
        where TController : MaaControllerBase
    {
        var testResponse = new object();
        var testResponseData = MaaApiResponseHelper.Ok(testResponse);
        var mediator = new Mock<IMediator>();
        mediator.Setup(x => x.Send(It.IsAny<It.IsAnyType>(), It.IsAny<CancellationToken>()).Result)
                .Returns(testResponseData);

        var controllerType = typeof(TController);

        // Get controller constructor which takes 1 argument of IMediator type.
        var ctor = controllerType.GetConstructor(new Type[] { typeof(IMediator) });
        var controller = (TController)ctor!.Invoke(new object[] { mediator.Object });

        var response = testMethod.Invoke(controller).GetAwaiter().GetResult();
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(new OkObjectResult(MaaApiResponseHelper.Ok(testResponse)));
    }
}
