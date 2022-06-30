// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;
using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.Test.TestHelpers;

/// <summary>
/// The helper class for testing handlers.
/// </summary>
[ExcludeFromCodeCoverage]
public class HandlerTest
{
    #region Mocks
    /// <summary>
    /// The API error message.
    /// </summary>
    public Resources.ApiErrorMessage ApiErrorMessage { get; } = new();

    /// <summary>
    /// The validation error message.
    /// </summary>
    public Resources.ValidationErrorMessage ValidationErrorMessage { get; } = new();

    /// <summary>
    /// The DB context.
    /// </summary>
    public IMaaCopilotDbContext DbContext { get; } = new TestDbContext();

    /// <summary>
    /// The current user service.
    /// </summary>
    public Mock<ICurrentUserService> CurrentUserService { get; private set; } = new();

    /// <summary>
    /// The secret service.
    /// </summary>
    public Mock<ISecretService> SecretService { get; private set; } = new();

    /// <summary>
    /// The mail service.
    /// </summary>
    public Mock<IMailService> MailService { get; private set; } = new();

    /// <summary>
    /// The token options.
    /// </summary>
    public IOptions<TokenOption> TokenOption { get; private set; } = Options.Create(new TokenOption()
    {
        AccountActivationToken = new TokenConfiguration()
        {
            ExpireTime = default,
            HasCallback = default,
        },
        PasswordResetToken = new TokenConfiguration()
        {
            ExpireTime = default,
            HasCallback = default,
        },
        ChangeEmailToken = new TokenConfiguration()
        {
            ExpireTime = default,
            HasCallback = default,
        },
    });

    /// <summary>
    /// The copilot server options.
    /// </summary>
    public IOptions<CopilotServerOption> CopilotServerOption { get; private set; } = Options.Create(new CopilotServerOption()
    {
        RegisterUserDefaultRole = Domain.Enums.UserRole.User,
        RequireTitleInOperation = default,
        RequireDetailsInOperation = default,
        EnableTestEmailApi = default,
        TestEmailApiToken = string.Empty,
    });
    #endregion

    #region Mock Setups
    /// <summary>
    /// Setups <see cref="DbContext"/>.
    /// </summary>
    /// <param name="action">The setup action.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupDatabase(Action<IMaaCopilotDbContext> action)
    {
        action.Invoke(DbContext);
        DbContext.SaveChangesAsync(new CancellationToken()).Wait();
        return this;
    }

    /// <summary>
    /// Setups <see cref="CurrentUserService"/>.
    /// </summary>
    /// <param name="action">The setup action.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupCurrentUserService(Action<Mock<ICurrentUserService>> action)
    {
        action.Invoke(CurrentUserService);
        return this;
    }

    /// <summary>
    /// Setups <see cref="CurrentUserService"/>.
    /// </summary>
    /// <param name="mock">The new mock.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupCurrentUserService(Mock<ICurrentUserService> mock)
    {
        CurrentUserService = mock;
        return this;
    }

    /// <summary>
    /// Setups <see cref="SecretService"/>.
    /// </summary>
    /// <param name="action">The setup action.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupSecretService(Action<Mock<ISecretService>> action)
    {
        action.Invoke(SecretService);
        return this;
    }

    /// <summary>
    /// Setups <see cref="SecretService"/>.
    /// </summary>
    /// <param name="mock">The new mock.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupSecretService(Mock<ISecretService> mock)
    {
        SecretService = mock;
        return this;
    }

    /// <summary>
    /// Setups <see cref="MailService"/>.
    /// </summary>
    /// <param name="action">The setup action.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupMailService(Action<Mock<IMailService>> action)
    {
        action.Invoke(MailService);
        return this;
    }

    /// <summary>
    /// Setups <see cref="MailService"/>.
    /// </summary>
    /// <param name="mock">The new mock.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupMailService(Mock<IMailService> mock)
    {
        MailService = mock;
        return this;
    }

    /// <summary>
    /// Setups <see cref="TokenOption"/>.
    /// </summary>
    /// <param name="newOption">The new option.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupTokenOption(TokenOption newOption)
    {
        TokenOption = Options.Create(newOption);
        return this;
    }

    /// <summary>
    /// Setups <see cref="CopilotServerOption"/>.
    /// </summary>
    /// <param name="newOption">The new option.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupCopilotServerOption(CopilotServerOption newOption)
    {
        CopilotServerOption = Options.Create(newOption);
        return this;
    }
    #endregion

    #region Testers

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestChangeCopilotUserInfo(ChangeCopilotUserInfoCommand request)
    {
        var handler = new ChangeCopilotUserInfoCommandHandler(DbContext, CurrentUserService.Object, SecretService.Object, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestPasswordReset(PasswordResetCommand request)
    {
        var handler = new PasswordResetCommandHandler(SecretService.Object, DbContext, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestRequestPasswordReset(RequestPasswordResetCommand request)
    {
        var handler = new RequestPasswordResetCommandHandler(TokenOption, DbContext, SecretService.Object, MailService.Object, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestUpdateCopilotUserInfo(UpdateCopilotUserInfoCommand request)
    {
        var handler = new UpdateCopilotUserInfoCommandHandler(TokenOption, DbContext, MailService.Object, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestUpdateCopilotUserPassword(UpdateCopilotUserPasswordCommand request)
    {
        var handler = new UpdateCopilotUserPasswordCommandHandler(DbContext, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestActivateCopilotAccount(ActivateCopilotAccountCommand request)
    {
        var handler = new ActivateCopilotAccountCommandHandler(CurrentUserService.Object, DbContext, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestCreateCopilotUser(CreateCopilotUserCommand request)
    {
        var handler = new CreateCopilotUserCommandHandler(DbContext, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The response.</returns>
    public MaaApiResponse TestRegisterCopilotAccount(RegisterCopilotAccountCommand request)
    {
        var handler = new RegisterCopilotAccountCommandHandler(TokenOption, CurrentUserService.Object, DbContext, SecretService.Object, MailService.Object, CopilotServerOption, ApiErrorMessage);
        return handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();
    }
    #endregion
}
