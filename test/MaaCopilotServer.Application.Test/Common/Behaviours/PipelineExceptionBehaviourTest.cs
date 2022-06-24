// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Test.Common.Behaviours;

/// <summary>
///     Tests of <see cref="PipelineExceptionBehaviour{TRequest,TResponse}" />.
/// </summary>
[TestClass]
public class PipelineExceptionBehaviourTest
{
    /// <summary>
    ///     The logger.
    /// </summary>
    private ILogger<IRequest<string>> _logger;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _logger = Substitute.For<ILogger<IRequest<string>>>();
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="PipelineExceptionBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, MediatR.RequestHandlerDelegate{TResponse})" />
    ///     .
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle()
    {
        var behaviour = new PipelineExceptionBehaviour<IRequest<string>, string>(_logger);
        var action = async () => await behaviour.Handle(null, new CancellationToken(), () =>
        {
            throw new PipelineException(MaaApiResponseHelper.Ok<GetCopilotUserDto>(new GetCopilotUserDto(), string.Empty));
        });
        await action.Should().ThrowAsync<PipelineException>();
    }
}
