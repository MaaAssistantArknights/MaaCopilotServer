// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.ChangeCopilotUserInfo;

/// <summary>
/// Tests for <see cref="ChangeCopilotUserInfoCommandHandler"/>.
/// </summary>
[TestClass]
public class ChangeCopilotUserInfoCommandTest
{
    /// <summary>
    /// The API error message.
    /// </summary>
    private readonly Resources.ApiErrorMessage _apiErrorMessage = new();

    /// <summary>
    /// The current user service.
    /// </summary>
    private readonly ICurrentUserService _currentUserService = Mock.Of<ICurrentUserService>();

    /// <summary>
    /// The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();

    /// <summary>
    /// The secret service.
    /// </summary>
    private readonly ISecretService _secretService = Mock.Of<ISecretService>();

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with non-existing user.
    /// </summary>
    [TestMethod]
    public void TestHandle_UserNotFound()
    {
        var request = new ChangeCopilotUserInfoCommand()
        {
            UserId = Guid.Empty.ToString(),
        };
        var handler = new ChangeCopilotUserInfoCommandHandler(_dbContext, _currentUserService, _secretService, _apiErrorMessage);
        var resposne = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        resposne.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with insufficient operator permission.
    /// </summary>
    /// <param name="userRole">The test user role.</param>
    /// <param name="operatorRole">The test operator role.</param>
    [DataTestMethod]
    [DataRow(Domain.Enums.UserRole.Admin, Domain.Enums.UserRole.Admin)]
    [DataRow(Domain.Enums.UserRole.SuperAdmin, Domain.Enums.UserRole.Admin)]
    public void TestHandle_OperatorPermissionDenied(Domain.Enums.UserRole userRole, Domain.Enums.UserRole operatorRole)
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, userRole, null);
        _dbContext.CopilotUsers.Add(user);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, operatorRole, null);
        _dbContext.CopilotUsers.Add(@operator);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(@operator);

        var request = new ChangeCopilotUserInfoCommand()
        {
            UserId = user.EntityId.ToString(),
        };
        var handler = new ChangeCopilotUserInfoCommandHandler(_dbContext, currentUserService.Object, _secretService, _apiErrorMessage);
        var resposne = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        resposne.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with super admin role changes.
    /// </summary>
    [TestMethod]
    public void TestHandle_SuperAdminRoleChanges()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);
        _dbContext.CopilotUsers.Add(user);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);
        _dbContext.CopilotUsers.Add(@operator);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(@operator);

        var request = new ChangeCopilotUserInfoCommand()
        {
            UserId = user.EntityId.ToString(),
            Role = Domain.Enums.UserRole.Admin.ToString(),
        };
        var handler = new ChangeCopilotUserInfoCommandHandler(_dbContext, currentUserService.Object, _secretService, _apiErrorMessage);
        var resposne = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        resposne.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with email address in use.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailInUse()
    {
        var user = new Domain.Entities.CopilotUser("user@example.com", string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.Admin, null);
        _dbContext.CopilotUsers.Add(@operator);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(@operator);

        var request = new ChangeCopilotUserInfoCommand()
        {
            UserId = user.EntityId.ToString(),
            Email = "user@example.com",
        };
        var handler = new ChangeCopilotUserInfoCommandHandler(_dbContext, currentUserService.Object, _secretService, _apiErrorMessage);
        var resposne = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        resposne.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with valid request.
    /// </summary>
    [TestMethod]
    public void TestHandle_Valid()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        user.ActivateUser(Guid.Empty);
        _dbContext.CopilotUsers.Add(user);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);
        _dbContext.CopilotUsers.Add(@operator);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(@operator);
        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.HashPassword("new_password")).Returns("hashed_password");

        var request = new ChangeCopilotUserInfoCommand()
        {
            UserId = user.EntityId.ToString(),
            Email = "user@example.com",
            UserName = "test_username",
            Password = "new_password",
            Role = Domain.Enums.UserRole.Uploader.ToString(),
        };
        var handler = new ChangeCopilotUserInfoCommandHandler(_dbContext, currentUserService.Object, secretService.Object, _apiErrorMessage);
        var resposne = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        resposne.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserActivated.Should().BeTrue();
        user.Email.Should().Be("user@example.com");
        user.UserName.Should().Be("test_username");
        user.Password.Should().Be("hashed_password");
        user.UserRole.Should().Be(Domain.Enums.UserRole.Uploader);
    }
}
