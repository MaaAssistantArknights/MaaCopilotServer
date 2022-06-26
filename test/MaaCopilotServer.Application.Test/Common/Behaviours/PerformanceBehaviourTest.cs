// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Test.Common.Behaviours;

/// <summary>
///     Tests of <see cref="PerformanceBehaviour{TRequest,TResponse}" />.
/// </summary>
[TestClass]
public class PerformanceBehaviourTest
{
    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="PerformanceBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     .
    /// </summary>
    /// <returns>N/A</returns>
    [DataTestMethod]
    [DataRow(StatusCodes.Status200OK, LogLevel.Information)]
    [DataRow(StatusCodes.Status400BadRequest, LogLevel.Warning)]
    [DataRow(StatusCodes.Status401Unauthorized, LogLevel.Warning)]
    [DataRow(StatusCodes.Status403Forbidden, LogLevel.Warning)]
    [DataRow(StatusCodes.Status404NotFound, LogLevel.Warning)]
    [DataRow(StatusCodes.Status500InternalServerError, LogLevel.Error)]
    public async Task TestHandle(int statusCode, LogLevel expectedLogLevel)
    {
        var testUserId = new Guid();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(testUserId);
        var logger = new Mock<ILogger<IRequest<MaaApiResponse>>>();

        var behaviour =
            new PerformanceBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(
                logger.Object, currentUserService.Object);
        await behaviour.Handle(default!, new CancellationToken(), () => Task.FromResult(new MaaApiResponse()
        {
            StatusCode = statusCode,
        }));

        logger.Verify(x => x.Log(
            expectedLogLevel,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
