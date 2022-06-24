// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;

using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
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

namespace MaaCopilotServer.Api.Test.Properties;

/// <summary>
///     Tests for <see cref="CopilotUserController" />.
/// </summary>
[TestClass]
public class CopilotUserControllerTest
{
    /// <summary>
    ///     The mock mediator.
    /// </summary>
    private IMediator _mediator;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _mediator = Substitute.For<IMediator>();
    }

    /// <summary>
    ///     Tests constructor.
    /// </summary>
    [TestMethod]
    public void TestConstructor()
    {
        var controller = new CopilotUserController(_mediator);
        controller.Should().NotBeNull();
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.ChangeCopilotUserInfo(ChangeCopilotUserInfoCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestChangeCopilotUserInfo()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<ChangeCopilotUserInfoCommand, object?>(
            _mediator,
            new ChangeCopilotUserInfoCommand(),
            null,
            controller.ChangeCopilotUserInfo);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.CreateCopilotUser(CreateCopilotUserCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestCreateCopilotUser()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<CreateCopilotUserCommand, object?>(
            _mediator,
            new CreateCopilotUserCommand(),
            null,
            controller.CreateCopilotUser);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.DeleteCopilotUser(DeleteCopilotUserCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestDeleteCopilotUser()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<DeleteCopilotUserCommand, object?>(
            _mediator,
            new DeleteCopilotUserCommand(),
            null,
            controller.DeleteCopilotUser);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.LoginCopilotUser(LoginCopilotUserCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestLoginCopilotUser()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint(
            _mediator,
            new LoginCopilotUserCommand(),
            new LoginCopilotUserDto(),
            controller.LoginCopilotUser);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.UpdateCopilotUserInfo(UpdateCopilotUserInfoCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestUpdateCopilotUserInfo()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<UpdateCopilotUserInfoCommand, object?>(
            _mediator,
            new UpdateCopilotUserInfoCommand(),
            null,
            controller.UpdateCopilotUserInfo);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.UpdateCopilotUserPassword(UpdateCopilotUserPasswordCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestUpdateCopilotUserPassword()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<UpdateCopilotUserPasswordCommand, object?>(
            _mediator,
            new UpdateCopilotUserPasswordCommand(),
            null,
            controller.UpdateCopilotUserPassword);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.GetCopilotUser(string?)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestGetCopilotUser()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint(
            _mediator,
            string.Empty,
            new GetCopilotUserDto(),
            controller.GetCopilotUser);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.QueryCopilotUser(QueryCopilotUserQuery)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestQueryCopilotUser()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint(
            _mediator,
            new QueryCopilotUserQuery(),
            new PaginationResult<QueryCopilotUserDto>(),
            controller.QueryCopilotUser);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.RegisterAccount(RegisterCopilotAccountCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestRegisterAccount()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<RegisterCopilotAccountCommand, object?>(
            _mediator,
            new RegisterCopilotAccountCommand(),
            null,
            controller.RegisterAccount);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.ActivateAccount(ActivateCopilotAccountCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestActivateAccount()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<ActivateCopilotAccountCommand, object?>(
            _mediator,
            new ActivateCopilotAccountCommand(),
            null,
            controller.ActivateAccount);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.RequestPasswordChange(RequestPasswordResetCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestRequestPasswordChange()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<RequestPasswordResetCommand, object?>(
            _mediator,
            new RequestPasswordResetCommand(),
            null,
            controller.RequestPasswordChange);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.PasswordChange(PasswordResetCommand)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestPasswordChange()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint<PasswordResetCommand, object?>(
            _mediator,
            new PasswordResetCommand(),
            null,
            controller.PasswordChange);
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.GetFavorites(string?)" />.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestGetFavorites()
    {
        var controller = new CopilotUserController(_mediator);
        await ControllerTestUtils.TestControllerEndpoint(
            _mediator,
            string.Empty,
            new GetCopilotUserFavoritesDto(),
            controller.GetFavorites);
    }
}
