// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;
using MaaCopilotServer.Api.Test.TestHelpers;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotFavorite.Queries.GetCopilotUserFavorites;
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
using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

namespace MaaCopilotServer.Api.Test.Properties;

/// <summary>
///     Tests for <see cref="CopilotUserController" />.
/// </summary>
[TestClass]
public class CopilotUserControllerTest
{
    /// <summary>
    ///     Tests <see cref="CopilotUserController.ChangeCopilotUserInfo(ChangeCopilotUserInfoCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestChangeCopilotUserInfo()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new ChangeCopilotUserInfoCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.ChangeCopilotUserInfo(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.CreateCopilotUser(CreateCopilotUserCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestCreateCopilotUser()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new CreateCopilotUserCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.CreateCopilotUser(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.DeleteCopilotUser(DeleteCopilotUserCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestDeleteCopilotUser()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new DeleteCopilotUserCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.DeleteCopilotUser(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.LoginCopilotUser(LoginCopilotUserCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestLoginCopilotUser()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new LoginCopilotUserCommand(),
            new LoginCopilotUserDto(),
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.LoginCopilotUser(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.UpdateCopilotUserInfo(UpdateCopilotUserInfoCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestUpdateCopilotUserInfo()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new UpdateCopilotUserInfoCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.UpdateCopilotUserInfo(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.UpdateCopilotUserPassword(UpdateCopilotUserPasswordCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestUpdateCopilotUserPassword()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new UpdateCopilotUserPasswordCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.UpdateCopilotUserPassword(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.GetCopilotUser(string?)" />.
    /// </summary>
    [TestMethod]
    public void TestGetCopilotUser()
    {
        ControllerTestHelper.TestControllerEndpoint(
            string.Empty,
            new GetCopilotUserDto(),
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.GetCopilotUser(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.QueryCopilotUser(QueryCopilotUserQuery)" />.
    /// </summary>
    [TestMethod]
    public void TestQueryCopilotUser()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new QueryCopilotUserQuery(),
            new PaginationResult<QueryCopilotUserDto>(),
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.QueryCopilotUser(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.RegisterAccount(RegisterCopilotAccountCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestRegisterAccount()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new RegisterCopilotAccountCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.RegisterAccount(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.ActivateAccount(ActivateCopilotAccountCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestActivateAccount()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new ActivateCopilotAccountCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.ActivateAccount(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.RequestPasswordChange(RequestPasswordResetCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestRequestPasswordChange()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new RequestPasswordResetCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.RequestPasswordChange(request));
    }

    /// <summary>
    ///     Tests <see cref="CopilotUserController.PasswordChange(PasswordResetCommand)" />.
    /// </summary>
    [TestMethod]
    public void TestPasswordChange()
    {
        ControllerTestHelper.TestControllerEndpoint(
            new PasswordResetCommand(),
            null,
            mediator => new CopilotUserController(mediator),
            (controller, request) => controller.PasswordChange(request));
    }
}
