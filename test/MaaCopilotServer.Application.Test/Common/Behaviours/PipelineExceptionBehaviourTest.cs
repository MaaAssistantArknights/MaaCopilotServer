// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Test.Common.Behaviours
{
    using System.Threading.Tasks;
    using MaaCopilotServer.Application.Common.Behaviours;
    using MaaCopilotServer.Application.Common.Exceptions;
    using MaaCopilotServer.Application.Common.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Tests of <see cref="PipelineExceptionBehaviour{TRequest, TResponse}"/>.
    /// </summary>
    [TestClass]
    public class PipelineExceptionBehaviourTest
    {
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
            this._logger = Substitute.For<ILogger<MediatR.IRequest<string>>>();
        }

        /// <summary>
        /// Tests <see cref="PipelineExceptionBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, MediatR.RequestHandlerDelegate{TResponse})"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestHandle()
        {
            var behaviour = new PipelineExceptionBehaviour<MediatR.IRequest<string>, string>(this._logger);
            var action = async () => await behaviour.Handle(null, new CancellationToken(), () =>
            {
                throw new PipelineException((MaaActionResult<EmptyObject>)MaaApiResponse.Ok(new EmptyObject(), string.Empty));
            });
            await action.Should().ThrowAsync<PipelineException>();
        }
    }
}
