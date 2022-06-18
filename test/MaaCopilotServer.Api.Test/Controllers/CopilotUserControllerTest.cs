// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Api.Test.Properties
{
    using MaaCopilotServer.Api.Controllers;
    using MaaCopilotServer.Application.Common.Exceptions;
    using MaaCopilotServer.Application.Common.Models;
    using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
    using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
    using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
    using MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;
    using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
    using MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;
    using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
    using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
    using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
    using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
    using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
    using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUserFavorites;
    using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
    using MediatR;

    /// <summary>
    /// Tests for <see cref="CopilotUserController"/>.
    /// </summary>
    [TestClass]
    public class CopilotUserControllerTest
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
            var controller = new CopilotUserController(this._mediator);
            controller.Should().NotBeNull();
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.ChangeCopilotUserInfo(ChangeCopilotUserInfoCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestChangeCopilotUserInfo()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new ChangeCopilotUserInfoCommand(),
                new EmptyObject(),
                controller.ChangeCopilotUserInfo);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.ChangeCopilotUserInfo(ChangeCopilotUserInfoCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestCreateCopilotOperation_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new ChangeCopilotUserInfoCommand(),
                controller.ChangeCopilotUserInfo);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.CreateCopilotUser(CreateCopilotUserCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestCreateCopilotUser()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new CreateCopilotUserCommand(),
                new EmptyObject(),
                controller.CreateCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.CreateCopilotUser(CreateCopilotUserCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestCreateCopilotUser_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new CreateCopilotUserCommand(),
                controller.CreateCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.DeleteCopilotUser(DeleteCopilotUserCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestDeleteCopilotUser()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new DeleteCopilotUserCommand(),
                new EmptyObject(),
                controller.DeleteCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.DeleteCopilotUser(DeleteCopilotUserCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestDeleteCopilotUser_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new DeleteCopilotUserCommand(),
                controller.DeleteCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.LoginCopilotUser(LoginCopilotUserCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestLoginCopilotUser()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new LoginCopilotUserCommand(),
                new LoginCopilotUserDto(),
                controller.LoginCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.LoginCopilotUser(LoginCopilotUserCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestLoginCopilotUser_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new LoginCopilotUserCommand(),
                controller.LoginCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.UpdateCopilotUserInfo(UpdateCopilotUserInfoCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestUpdateCopilotUserInfo()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new UpdateCopilotUserInfoCommand(),
                new EmptyObject(),
                controller.UpdateCopilotUserInfo);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.UpdateCopilotUserInfo(UpdateCopilotUserInfoCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestUpdateCopilotUserInfo_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new UpdateCopilotUserInfoCommand(),
                controller.UpdateCopilotUserInfo);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.UpdateCopilotUserPassword(UpdateCopilotUserPasswordCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestUpdateCopilotUserPassword()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new UpdateCopilotUserPasswordCommand(),
                new EmptyObject(),
                controller.UpdateCopilotUserPassword);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.UpdateCopilotUserPassword(UpdateCopilotUserPasswordCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestUpdateCopilotUserPassword_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new UpdateCopilotUserPasswordCommand(),
                controller.UpdateCopilotUserPassword);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.GetCopilotUser(string?)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestGetCopilotUser()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                string.Empty,
                new GetCopilotUserDto(),
                controller.GetCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.GetCopilotUser(string?)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestGetCopilotUser_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                string.Empty,
                controller.GetCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.QueryCopilotUser(QueryCopilotUserQuery)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestQueryCopilotUser()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new QueryCopilotUserQuery(),
                new PaginationResult<QueryCopilotUserDto>(default, default, default, default),
                controller.QueryCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.QueryCopilotUser(QueryCopilotUserQuery)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestQueryCopilotUser_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new QueryCopilotUserQuery(),
                controller.QueryCopilotUser);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.RegisterAccount(RegisterCopilotAccountCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestRegisterAccount()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new RegisterCopilotAccountCommand(),
                new EmptyObject(),
                controller.RegisterAccount);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.RegisterAccount(RegisterCopilotAccountCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestRegisterAccount_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new RegisterCopilotAccountCommand(),
                controller.RegisterAccount);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.ActivateAccount(ActivateCopilotAccountCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestActivateAccount()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new ActivateCopilotAccountCommand(),
                new EmptyObject(),
                controller.ActivateAccount);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.ActivateAccount(ActivateCopilotAccountCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestRequestPasswordChange_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new RequestPasswordResetCommand(),
                controller.RequestPasswordChange);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.RequestPasswordChange(RequestPasswordResetCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestRequestPasswordChange()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new RequestPasswordResetCommand(),
                new EmptyObject(),
                controller.RequestPasswordChange);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.RequestPasswordChange(RequestPasswordResetCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestActivateAccount_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new RequestPasswordResetCommand(),
                controller.RequestPasswordChange);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.PasswordChange(PasswordResetCommand)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestPasswordChange()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                new PasswordResetCommand(),
                new EmptyObject(),
                controller.PasswordChange);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.PasswordChange(PasswordResetCommand)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestPasswordChange_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                new PasswordResetCommand(),
                controller.PasswordChange);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.GetFavorites(string?)"/>.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestGetFavorites()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpoint(
                this._mediator,
                string.Empty,
                new GetCopilotUserFavoritesDto(),
                controller.GetFavorites);
        }

        /// <summary>
        /// Tests <see cref="CopilotUserController.GetFavorites(string?)"/> with <see cref="PipelineException"/> thrown.
        /// </summary>
        /// <returns>N/A</returns>
        [TestMethod]
        public async Task TestGetFavorites_WithException()
        {
            var controller = new CopilotUserController(this._mediator);
            await ControllerTestUtils.TestControllerEndpointWithException(
                this._mediator,
                string.Empty,
                controller.GetFavorites);
        }
    }
}
