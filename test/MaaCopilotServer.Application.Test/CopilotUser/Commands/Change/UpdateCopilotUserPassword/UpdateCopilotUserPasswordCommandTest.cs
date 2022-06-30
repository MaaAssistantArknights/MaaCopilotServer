// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.UpdateCopilotUserPassword;

/// <summary>
/// Tests of <see cref="UpdateCopilotUserPasswordCommandHandler"/>.
/// </summary>
[TestClass]
public class UpdateCopilotUserPasswordCommandTest
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
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/> with wrong original password.
    /// </summary>
    [TestMethod]
    public void TestHandle_OriginalPasswordWrong()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, "old_password_hash", string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.VerifyPassword("old_password_hash", "wrong_password")).Returns(false);

        var request = new UpdateCopilotUserPasswordCommand()
        {
            OriginalPassword = "wrong_password",
        };
        var handler = new UpdateCopilotUserPasswordCommandHandler(_dbContext, secretService.Object, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/> with wrong original password.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, "old_password_hash", string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var secretService = new Mock<ISecretService>();
        secretService.Setup(x => x.VerifyPassword("old_password_hash", "old_password")).Returns(true);
        secretService.Setup(x => x.HashPassword("new_password")).Returns("new_password_hash");

        var request = new UpdateCopilotUserPasswordCommand()
        {
            OriginalPassword = "old_password",
            NewPassword = "new_password",
        };
        var handler = new UpdateCopilotUserPasswordCommandHandler(_dbContext, secretService.Object, currentUserService.Object, _apiErrorMessage);
        var response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Password.Should().Be("new_password_hash");
    }
}
