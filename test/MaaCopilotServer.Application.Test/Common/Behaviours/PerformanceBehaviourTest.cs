// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Test.Common.Behaviours
{
    using System.Threading.Tasks;
    using MaaCopilotServer.Application.Common.Behaviours;
    using MaaCopilotServer.Application.Common.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Tests of <see cref="PerformanceBehaviour{TRequest, TResponse}"/>.
    /// </summary>
    [TestClass]
    public class PerformanceBehaviourTest
    {
        /// <summary>
        /// The service of current user.
        /// </summary>
        private ICurrentUserService _currentUserService;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger<MediatR.IRequest<string>> _logger;

        /// <summary>
        /// Initializes tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this._currentUserService = Substitute.For<ICurrentUserService>();
            this._logger = Substitute.For<ILogger<MediatR.IRequest<string>>>();
        }

        /// <summary>
        /// Tests <see cref="PerformanceBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, MediatR.RequestHandlerDelegate{TResponse})"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestHandle()
        {
            var testUserId = new Guid();
            this._currentUserService.GetUserIdentity().Returns(testUserId);
            var behaviour = new PerformanceBehaviour<MediatR.IRequest<string>, string>(this._logger, this._currentUserService);
            var action = async () => await behaviour.Handle(null, new CancellationToken(), () => Task.FromResult(string.Empty));
            await action.Should().NotThrowAsync();
        }
    }
}
