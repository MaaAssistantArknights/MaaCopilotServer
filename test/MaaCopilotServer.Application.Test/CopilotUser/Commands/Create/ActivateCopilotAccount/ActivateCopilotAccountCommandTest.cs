// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Test.TestHelpers;
using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.ActivateCopilotAccount;

/// <summary>
/// Tests of <see cref="ActivateCopilotAccountCommandHandler"/>.
/// </summary>
[TestClass]
public class ActivateCopilotAccountCommandTest
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
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with not existing token.
    /// </summary>
    [TestMethod]
    public void TestHandle_NotExistingToken()
    {
        var handler = new ActivateCopilotAccountCommandHandler(default!, _dbContext, _apiErrorMessage);
        var request = new ActivateCopilotAccountCommand()
        {
            Token = "not_existing_token",
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with expired token.
    /// </summary>
    [TestMethod]
    public void TestHandle_ExpiredToken()
    {
        var token = new Domain.Entities.CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserActivation, "token", new DateTimeOffset(1900, 1, 1, 0, 0, 0, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var handler = new ActivateCopilotAccountCommandHandler(default!, _dbContext, _apiErrorMessage);
        var request = new ActivateCopilotAccountCommand()
        {
            Token = "token",
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with token of wrong type.
    /// </summary>
    [TestMethod]
    public void TestHandle_WrongTypeToken()
    {
        var token = new Domain.Entities.CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserPasswordReset, "token", new DateTimeOffset(9999, 12, 31, 23, 59, 59, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var handler = new ActivateCopilotAccountCommandHandler(default!, _dbContext, _apiErrorMessage);
        var request = new ActivateCopilotAccountCommand()
        {
            Token = "token",
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with user not found.
    /// </summary>
    [TestMethod]
    public void TestHandle_UserNotFound()
    {
        var token = new Domain.Entities.CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserActivation, "token", new DateTimeOffset(9999, 12, 31, 23, 59, 59, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var handler = new ActivateCopilotAccountCommandHandler(default!, _dbContext, _apiErrorMessage);
        var request = new ActivateCopilotAccountCommand()
        {
            Token = "token",
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var token = new Domain.Entities.CopilotToken(user.EntityId, Domain.Enums.TokenType.UserActivation, "token", new DateTimeOffset(9999, 12, 31, 23, 59, 59, default));
        _dbContext.CopilotTokens.Add(token);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var handler = new ActivateCopilotAccountCommandHandler(default!, _dbContext, _apiErrorMessage);
        var request = new ActivateCopilotAccountCommand()
        {
            Token = "token",
        };
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserActivated.Should().BeTrue();
        _dbContext.CopilotTokens.FirstOrDefault(x => x.Token == "token").Should().BeNull();
    }
}
