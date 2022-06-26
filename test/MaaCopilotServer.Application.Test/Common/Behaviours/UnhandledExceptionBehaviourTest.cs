// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Resources;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Test.Common.Behaviours;

/// <summary>
///     Tests of <see cref="UnhandledExceptionBehaviour{TRequest,TResponse}" />.
/// </summary>
[TestClass]
public class UnhandledExceptionBehaviourTest
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage = new();

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="UnhandledExceptionBehaviour{TRequest}.Handle(TRequest, CancellationToken, MediatR.RequestHandlerDelegate{MaaApiResponse})" />
    ///     with <see cref="Exception" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_Exception()
    {
        var logger = new Mock<ILogger<IRequest<MaaApiResponse>>>();

        var behaviour =
            new UnhandledExceptionBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(
                logger.Object, _apiErrorMessage);
        var response = await behaviour.Handle(default!, new CancellationToken(), () =>
        {
            throw new Exception();
        });

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
