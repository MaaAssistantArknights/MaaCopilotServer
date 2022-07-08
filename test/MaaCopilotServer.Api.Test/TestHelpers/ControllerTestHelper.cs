// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Api.Controllers;
using MaaCopilotServer.Application.Common.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Test.TestHelpers;

/// <summary>
/// The helper class for testing controllers.
/// </summary>
[ExcludeFromCodeCoverage]
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
        var controllerType = typeof(TController);

        // Get controller constructor which takes 1 argument of IMediator type.
        var ctor = controllerType.GetConstructor(new Type[] { typeof(IMediator) });

        // Test controller.
        Test(mediator => (TController)ctor!.Invoke(new object[] { mediator }), testMethod);
    }

    /// <summary>
    ///     Tests an endpoint of the controller.
    /// </summary>
    /// <typeparam name="TController">
    ///     The type of the controller, which should be a derived class of <see cref="MaaControllerBase"/>.
    /// </typeparam>
    /// <param name="controllerBuilder">The builder function of the controller.</param>
    /// <param name="testMethod">The function to call a controller method.</param>
    /// <exception cref="ArgumentNullException">Thrown when the arguments are null.</exception>
    public static void Test<TController>(Func<IMediator, TController> controllerBuilder, Func<TController, Task<ActionResult>> testMethod)
        where TController : MaaControllerBase
    {
        if (controllerBuilder == null)
        {
            throw new ArgumentNullException(nameof(controllerBuilder));
        }

        if (testMethod == null)
        {
            throw new ArgumentNullException(nameof(testMethod));
        }

        var testResponse = new object();
        var testResponseData = MaaApiResponseHelper.Ok(testResponse);
        var mediator = new Mock<IMediator>();
        mediator.Setup(x => x.Send(It.IsAny<It.IsAnyType>(), It.IsAny<CancellationToken>()).Result)
                .Returns(testResponseData);

        var controller = controllerBuilder(mediator.Object);

        var response = testMethod.Invoke(controller).GetAwaiter().GetResult();
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(new OkObjectResult(MaaApiResponseHelper.Ok(testResponse)));
    }
}
