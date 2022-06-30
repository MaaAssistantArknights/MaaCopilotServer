// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.CreateCopilotUser;

/// <summary>
/// Tests of <see cref="CreateCopilotUserCommandHandler"/>.
/// </summary>
[TestClass]
public class CreateCopilotUserCommandHandlerTest
{
    private readonly Resources.ApiErrorMessage _apiErrorMessage = new();
    private readonly ICurrentUserService _currentUserService = Mock.Of<ICurrentUserService>();
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();
    private readonly ISecretService _secretService = Mock.Of<ISecretService>();

    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler.Handle(CreateCopilotUserCommand, CancellationToken)"/> with email in use.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailInUse()
    {
        var user = new Domain.Entities.CopilotUser("user@example.com", string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var request = new CreateCopilotUserCommand()
        {
            Email = "user@example.com",
        };
        var handler = new CreateCopilotUserCommandHandler(_dbContext, _secretService, _currentUserService, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler.Handle(CreateCopilotUserCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var adminUserEntity = Guid.NewGuid();

        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.HashPassword("password")).Returns("hashed_password");

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(adminUserEntity);

        var request = new CreateCopilotUserCommand()
        {
            Email = "user@example.com",
            Password = "password",
            UserName = "username",
            Role = "User",
        };
        var handler = new CreateCopilotUserCommandHandler(_dbContext, secretService.Object, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var user = _dbContext.CopilotUsers.FirstOrDefault(x => x.Email == "user@example.com");
        user.Should().NotBeNull();
        user!.Password.Should().Be("hashed_password");
        user.UserName.Should().Be(request.UserName);
        user.UserRole.Should().Be(Domain.Enums.UserRole.User);
        user.UserActivated.Should().BeTrue();
    }
}
