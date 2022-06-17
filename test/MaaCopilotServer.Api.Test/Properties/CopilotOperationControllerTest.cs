// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Api.Test.Properties
{
    using MaaCopilotServer.Api.Controllers;
    using MaaCopilotServer.Application.Common.Exceptions;
    using MaaCopilotServer.Application.Common.Models;
    using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
    using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
    using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
    using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
    using MediatR;

    /// <summary>
    /// Tests for <see cref="CopilotOperationController"/>.
    /// </summary>
    [TestClass]
    public class CopilotOperationControllerTest
    {
        /// <summary>
        /// The mock mediator.
        /// </summary>
        private IMediator _mediator;

        /// <summary>
        /// Initializes tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this._mediator = Substitute.For<IMediator>();
        }

        /// <summary>
        /// Tests constructor.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var controller = new CopilotOperationController(this._mediator);
            controller.Should().NotBeNull();
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.CreateCopilotOperation(CreateCopilotOperationCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestCreateCopilotOperation()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new CreateCopilotOperationCommand(),
                new CreateCopilotOperationDto(string.Empty),
                controller.CreateCopilotOperation);
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.CreateCopilotOperation(CreateCopilotOperationCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestCreateCopilotOperation_WithException()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new CreateCopilotOperationCommand(),
                controller.CreateCopilotOperation);
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.DeleteCopilotOperation(DeleteCopilotOperationCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestDeleteCopilotOperation()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new DeleteCopilotOperationCommand(),
                new EmptyObject(),
                controller.DeleteCopilotOperation);
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.CreateCopilotOperation(CreateCopilotOperationCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestDeleteCopilotOperation_WithException()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new DeleteCopilotOperationCommand(),
                controller.DeleteCopilotOperation);
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.GetCopilotOperation(string)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestGetCopilotOperation()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                string.Empty,
                new GetCopilotOperationQueryDto(default, default, default, default, default, default, default, default, default),
                controller.GetCopilotOperation);
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.GetCopilotOperation(string)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestGetCopilotOperation_WithException()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                string.Empty,
                controller.GetCopilotOperation);
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.QueryCopilotOperation(QueryCopilotOperationsQuery)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestQueryCopilotOperation()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new QueryCopilotOperationsQuery(),
                new PaginationResult<QueryCopilotOperationsQueryDto>(default, default, default, default),
                controller.QueryCopilotOperation);
        }

        /// <summary>
        /// Tests <see cref="CopilotOperationController.QueryCopilotOperation(QueryCopilotOperationsQuery)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestQueryCopilotOperation_WithException()
        {
            var controller = new CopilotOperationController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new QueryCopilotOperationsQuery(),
                controller.QueryCopilotOperation);
        }
    }
}
