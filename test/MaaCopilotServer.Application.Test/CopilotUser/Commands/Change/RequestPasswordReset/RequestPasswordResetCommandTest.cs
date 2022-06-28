// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.RequestPasswordReset;

/// <summary>
/// Tests for <see cref="RequestPasswordResetCommandHandler"/>.
/// </summary>
[TestClass]
public class RequestPasswordResetCommandTest
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
        PasswordResetToken = new TokenConfiguration()
        {
            ExpireTime = 0,
            HasCallback = false,
        }
    });

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with non-existing email.
    /// </summary>
    [TestMethod]
    public void TestHandle_UserNotFound()
    {
        var request = new RequestPasswordResetCommand()
        {
            Email = "not_exist"
        };
        var handler = new RequestPasswordResetCommandHandler(_tokenOption, _dbContext, _secretService, _mailService, _apiErrorMessage);

        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with the case when the email is not sent successfully.
    /// </summary>
    [TestMethod]
    public void TestHandle_SendingEmailFailed()
    {
        var user = new Domain.Entities.CopilotUser("user@example.com", string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var request = new RequestPasswordResetCommand()
        {
            Email = "user@example.com"
        };
        var secretService = new Mock<ISecretService>();
        secretService
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<TimeSpan>()))
            .Returns(("new_token", new DateTimeOffset(2020, 12, 31, 23, 59, 59, default)));
        var mailService = new Mock<IMailService>();
        mailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailPasswordReset>(), It.IsAny<string>()).Result).Returns(false);
        var handler = new RequestPasswordResetCommandHandler(_tokenOption, _dbContext, secretService.Object, mailService.Object, _apiErrorMessage);

        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with the successful case.
    /// </summary>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestHandle_Successful(bool alreadyHaveToken)
    {
        var user = new Domain.Entities.CopilotUser("user@example.com", string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        if (alreadyHaveToken)
        {
            var oldToken = new Domain.Entities.CopilotToken(user.EntityId, Domain.Enums.TokenType.UserPasswordReset, "old_token", new DateTimeOffset());
            _dbContext.CopilotTokens.Add(oldToken);
            _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        }
        var request = new RequestPasswordResetCommand()
        {
            Email = "user@example.com"
        };
        var secretService = new Mock<ISecretService>();
        secretService
            .Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<TimeSpan>()))
            .Returns(("new_token", new DateTimeOffset(2020, 12, 31, 23, 59, 59, default)));
        var mailService = new Mock<IMailService>();
        mailService.Setup(x => x.SendEmailAsync(It.IsAny<EmailPasswordReset>(), It.IsAny<string>()).Result).Returns(true);
        var handler = new RequestPasswordResetCommandHandler(_tokenOption, _dbContext, secretService.Object, mailService.Object, _apiErrorMessage);

        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var token = _dbContext.CopilotTokens.FirstOrDefault(
            x => x.ResourceId == user.EntityId && x.Type == Domain.Enums.TokenType.UserPasswordReset);
        token.Should().NotBeNull();
        token!.Token.Should().Be("new_token");
    }
}
