// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Test.Common.Behaviours
{
    using System.Threading.Tasks;
    using MaaCopilotServer.Application.Common.Behaviours;
    using MaaCopilotServer.Application.Common.Exceptions;
    using MaaCopilotServer.Application.Common.Interfaces;
    using MaaCopilotServer.Application.Common.Models;
    using MaaCopilotServer.Resources;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Tests of <see cref="UnhandledExceptionBehaviour{TRequest, TResponse}"/>.
    /// </summary>
    [TestClass]
    public class UnhandledExceptionBehaviourTest
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<MediatR.IRequest<string>> _logger;

        /// <summary>
        /// The service of current user.
        /// </summary>
        private ICurrentUserService _currentUserService;

        /// <summary>
        /// The API error message.
        /// </summary>
        private ApiErrorMessage _apiErrorMessage;

        /// <summary>
        /// Initializes tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this._logger = Substitute.For<ILogger<MediatR.IRequest<string>>>();
            this._currentUserService = Substitute.For<ICurrentUserService>();
            this._apiErrorMessage = Substitute.For<ApiErrorMessage>();
        }

        /// <summary>
        /// Tests <see cref="UnhandledExceptionBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, MediatR.RequestHandlerDelegate{TResponse})"/> with <see cref="PipelineException"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestHandle_PipelineException()
        {
            var behaviour = new UnhandledExceptionBehaviour<MediatR.IRequest<string>, string>(this._logger, this._currentUserService, this._apiErrorMessage);
            var action = async () => await behaviour.Handle(null, new CancellationToken(), () =>
            {
                throw new PipelineException((MaaActionResult<EmptyObject>)MaaApiResponse.Ok(new EmptyObject(), string.Empty));
            });
            await action.Should().ThrowAsync<PipelineException>();
        }

        /// <summary>
        /// Tests <see cref="UnhandledExceptionBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, MediatR.RequestHandlerDelegate{TResponse})"/> with <see cref="Exception"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestHandle_OtherException()
        {
            var behaviour = new UnhandledExceptionBehaviour<MediatR.IRequest<string>, string>(this._logger, this._currentUserService, this._apiErrorMessage);
            var action = async () => await behaviour.Handle(null, new CancellationToken(), () =>
            {
                throw new Exception();
            });
            await action.Should().ThrowAsync<PipelineException>();
        }
    }
}