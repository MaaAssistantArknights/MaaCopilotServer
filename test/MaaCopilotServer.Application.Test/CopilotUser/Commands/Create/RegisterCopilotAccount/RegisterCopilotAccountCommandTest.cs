// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.RegisterCopilotAccount;

/// <summary>
/// Tests of <see cref="RegisterCopilotAccountCommandHandler"/>.
/// </summary>
[TestClass]
public class RegisterCopilotAccountCommandTest
{
    private readonly Resources.ApiErrorMessage _apiErrorMessage = new();
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();
    private readonly IMailService _mailService = Mock.Of<IMailService>();
    private readonly IOptions<CopilotServerOption> _copilotServerOption = Options.Create(new CopilotServerOption()
    {
        RegisterUserDefaultRole = Domain.Enums.UserRole.User,
    });
    private readonly ISecretService _secretService = Mock.Of<ISecretService>();
    private readonly IOptions<TokenOption> _tokenOption = Options.Create(new TokenOption()
    {
        AccountActivationToken = new TokenConfiguration()
        {
            ExpireTime = default,
            HasCallback = default,
        },
    });

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with email in use.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailInUse()
    {
        var user = new Domain.Entities.CopilotUser("user@example.com", string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var request = new RegisterCopilotAccountCommand()
        {
            Email = "user@example.com",
        };
        var handler = new RegisterCopilotAccountCommandHandler(_tokenOption, default!, _dbContext, _secretService, _mailService, _copilotServerOption, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailFailedToSend()
    {
        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.HashPassword("password")).Returns("hashed_password");
        secretService.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<TimeSpan>())).Returns(("token", new DateTimeOffset(2020, 1, 1, 0, 0, 0, default)));

        var mailService = new Mock<IMailService>();
        mailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailUserActivation>(), "user@example.com").Result).Returns(false);

        var request = new RegisterCopilotAccountCommand()
        {
            Email = "user@example.com",
            UserName = "test_username",
            Password = "password",
        };
        var handler = new RegisterCopilotAccountCommandHandler(_tokenOption, default!, _dbContext, secretService.Object, mailService.Object, _copilotServerOption, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        _dbContext.CopilotUsers.Should().BeEmpty();
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.HashPassword("password")).Returns("hashed_password");
        secretService.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<TimeSpan>())).Returns(("token", new DateTimeOffset(2020, 1, 1, 0, 0, 0, default)));

        var mailService = new Mock<IMailService>();
        mailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailUserActivation>(), "user@example.com").Result).Returns(true);

        var request = new RegisterCopilotAccountCommand()
        {
            Email = "user@example.com",
            UserName = "test_username",
            Password = "password",
        };
        var handler = new RegisterCopilotAccountCommandHandler(_tokenOption, default!, _dbContext, secretService.Object, mailService.Object, _copilotServerOption, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var token = _dbContext.CopilotTokens.FirstOrDefault();
        token.Should().NotBeNull();
        token!.Type.Should().Be(Domain.Enums.TokenType.UserActivation);
        token.Token.Should().Be("token");
        var userEntity = token.ResourceId;
        var user = _dbContext.CopilotUsers.FirstOrDefault();
        user.Should().NotBeNull();
        user!.EntityId.Should().Be(userEntity);
        user.Email.Should().Be(request.Email);
        user.UserName.Should().Be(request.UserName);
        user.Password.Should().Be("hashed_password");
        user.UserRole.Should().Be(Domain.Enums.UserRole.User);
        user.UserActivated.Should().BeFalse();
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with default role higher than <see cref="Domain.Enums.UserRole.Uploader"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle_DefaultRoleTooHigh()
    {
        var copilotServerOption = Options.Create(new CopilotServerOption()
        {
            RegisterUserDefaultRole = Domain.Enums.UserRole.Admin,
        });

        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.HashPassword("password")).Returns("hashed_password");
        secretService.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<TimeSpan>())).Returns(("token", new DateTimeOffset(2020, 1, 1, 0, 0, 0, default)));

        var mailService = new Mock<IMailService>();
        mailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailUserActivation>(), "user@example.com").Result).Returns(true);

        var request = new RegisterCopilotAccountCommand()
        {
            Email = "user@example.com",
            UserName = "test_username",
            Password = "password",
        };
        var handler = new RegisterCopilotAccountCommandHandler(_tokenOption, default!, _dbContext, secretService.Object, mailService.Object, copilotServerOption, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var token = _dbContext.CopilotTokens.FirstOrDefault();
        token.Should().NotBeNull();
        token!.Type.Should().Be(Domain.Enums.TokenType.UserActivation);
        token.Token.Should().Be("token");
        var userEntity = token.ResourceId;
        var user = _dbContext.CopilotUsers.FirstOrDefault();
        user.Should().NotBeNull();
        user!.EntityId.Should().Be(userEntity);
        user.Email.Should().Be(request.Email);
        user.UserName.Should().Be(request.UserName);
        user.Password.Should().Be("hashed_password");
        user.UserRole.Should().Be(Domain.Enums.UserRole.Uploader);
        user.UserActivated.Should().BeFalse();
    }
}
