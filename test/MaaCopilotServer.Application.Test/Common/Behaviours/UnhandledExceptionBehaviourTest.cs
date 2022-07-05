// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Resources;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Test.Common.Behaviours;

/// <summary>
///     Tests <see cref="UnhandledExceptionBehaviour{TRequest,TResponse}" />.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
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
    [TestMethod]
    public void TestHandleException()
    {
        var logger = new Mock<ILogger<IRequest<MaaApiResponse>>>();

        var behaviour =
            new UnhandledExceptionBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(
                logger.Object, _apiErrorMessage);
        var response = behaviour.Handle(default!, new CancellationToken(), () =>
        {
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new Exception();
#pragma warning restore CA2201 // Do not raise reserved exception types
        }).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}
