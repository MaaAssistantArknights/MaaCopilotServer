// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Interfaces;
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
    private ApiErrorMessage _apiErrorMessage;

    /// <summary>
    ///     The service of current user.
    /// </summary>
    private ICurrentUserService _currentUserService;

    /// <summary>
    ///     The logger.
    /// </summary>
    private ILogger<IRequest<MaaApiResponse>> _logger;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _logger = Substitute.For<ILogger<IRequest<MaaApiResponse>>>();
        _currentUserService = Substitute.For<ICurrentUserService>();
        _apiErrorMessage = Substitute.For<ApiErrorMessage>();
    }

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
        var behaviour =
            new UnhandledExceptionBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_logger, _currentUserService, _apiErrorMessage);
        var response = await behaviour.Handle(null, new CancellationToken(), () =>
        {
            throw new Exception();
        });
        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}
