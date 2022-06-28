// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.


using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.PasswordReset;

/// <summary>
/// Tests for <see cref="PasswordResetCommandHandler"/>.
/// </summary>
[TestClass]
public class PasswordResetCommandTest
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
    /// The secret service.
    /// </summary>
    private readonly ISecretService _secretService = Mock.Of<ISecretService>();

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with invalid token.
    /// </summary>
    [TestMethod]
    public void TestHandle_InvalidToken()
    {
        var handler = new PasswordResetCommandHandler(_secretService, _dbContext, _apiErrorMessage);
        var request = new PasswordResetCommand()
        {
            Token = "invalid"
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with expired token.
    /// </summary>
    [TestMethod]
    public void TestHandle_ExpiredToken()
    {
        var token = new CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserPasswordReset, "token", new DateTimeOffset(1900, 1, 1, 0, 0, 0, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var handler = new PasswordResetCommandHandler(_secretService, _dbContext, _apiErrorMessage);
        var request = new PasswordResetCommand()
        {
            Token = "token"
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with token of wrong type.
    /// </summary>
    [TestMethod]
    public void TestHandle_WrongTypeToken()
    {
        var token = new CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserActivation, "token", new DateTimeOffset(9999, 12, 31, 23, 59, 59, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var handler = new PasswordResetCommandHandler(_secretService, _dbContext, _apiErrorMessage);
        var request = new PasswordResetCommand()
        {
            Token = "token"
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with invalid user.
    /// </summary>
    [TestMethod]
    public void TestHandle_InvalidUser()
    {
        var token = new CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserPasswordReset, "token", new DateTimeOffset(9999, 12, 31, 23, 59, 59, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var handler = new PasswordResetCommandHandler(_secretService, _dbContext, _apiErrorMessage);
        var request = new PasswordResetCommand()
        {
            Token = "token",
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with valid data.
    /// </summary>
    [TestMethod]
    public void TestHandle_Valid()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var token = new CopilotToken(user.EntityId, Domain.Enums.TokenType.UserPasswordReset, "token", new DateTimeOffset(9999, 12, 31, 23, 59, 59, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.HashPassword("new_password")).Returns("hashed_password");

        var handler = new PasswordResetCommandHandler(secretService.Object, _dbContext, _apiErrorMessage);
        var request = new PasswordResetCommand()
        {
            Token = "token",
            Password = "new_password",
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Password.Should().Be("hashed_password");
    }
}
