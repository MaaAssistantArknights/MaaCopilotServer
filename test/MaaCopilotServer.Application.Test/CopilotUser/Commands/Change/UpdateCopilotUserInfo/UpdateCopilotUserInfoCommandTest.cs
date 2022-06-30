// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.UpdateCopilotUserInfo;

/// <summary>
/// Tests for <see cref="UpdateCopilotUserInfoCommandHandler"/>.
/// </summary>
[TestClass]
public class UpdateCopilotUserInfoCommandTest
{
    /// <summary>
    /// The API error message.
    /// </summary>
    private readonly Resources.ApiErrorMessage _apiErrorMessage = new();

    /// <summary>
    /// The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();

    /// <summary>
    /// The mail service.
    /// </summary>
    private readonly IMailService _mailService = Mock.Of<IMailService>();

    /// <summary>
    /// The secret service.
    /// </summary>
    private readonly ISecretService _secretService = Mock.Of<ISecretService>();

    /// <summary>
    /// The token options.
    /// </summary>
    private readonly IOptions<TokenOption> _tokenOption = Options.Create(new TokenOption()
    {
        ChangeEmailToken = new TokenConfiguration()
        {
            ExpireTime = default,
        },
        AccountActivationToken = new TokenConfiguration()
        {
            HasCallback = default,
        },
    });

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with username changes.
    /// </summary>
    [TestMethod]
    public void TestHandle_ChangeUsername()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var request = new UpdateCopilotUserInfoCommand()
        {
            UserName = "new_username"
        };
        var handler = new UpdateCopilotUserInfoCommandHandler(_tokenOption, _dbContext, _mailService, _secretService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserName.Should().Be("new_username");
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes,
    /// but email is already in use.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailAlreadyInUse()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var user2 = new Domain.Entities.CopilotUser("user@example.com", string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.CopilotUsers.Add(user2);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var request = new UpdateCopilotUserInfoCommand()
        {
            Email = "user@example.com"
        };
        var handler = new UpdateCopilotUserInfoCommandHandler(_tokenOption, _dbContext, _mailService, _secretService, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes,
    /// but the activation email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandle_ActivationEmailFailedToSend()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var secretService = new Mock<ISecretService>();
        secretService
            .Setup(x => x.GenerateToken(user.EntityId, It.IsAny<TimeSpan>()))
            .Returns(("new_token", new DateTimeOffset(2020, 12, 31, 23, 59, 59, default)));

        var mailService = new Mock<IMailService>();
        mailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailChangeAddress>(), It.IsAny<string>()).Result).Returns(false);

        var request = new UpdateCopilotUserInfoCommand()
        {
            Email = "user@example.com"
        };
        var handler = new UpdateCopilotUserInfoCommandHandler(_tokenOption, _dbContext, mailService.Object, secretService.Object, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes.
    /// </summary>
    [TestMethod]
    public void TestHandle_ChangeEmail()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        user.ActivateUser(Guid.Empty);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var secretService = new Mock<ISecretService>();
        secretService
            .Setup(x => x.GenerateToken(user.EntityId, It.IsAny<TimeSpan>()))
            .Returns(("new_token", new DateTimeOffset(2020, 12, 31, 23, 59, 59, default)));

        var mailService = new Mock<IMailService>();
        mailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailChangeAddress>(), It.IsAny<string>()).Result).Returns(true);

        var request = new UpdateCopilotUserInfoCommand()
        {
            Email = "user@example.com"
        };
        var handler = new UpdateCopilotUserInfoCommandHandler(_tokenOption, _dbContext, mailService.Object, secretService.Object, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Email.Should().Be("user@example.com");
        user.UserActivated.Should().BeFalse();
    }
}
