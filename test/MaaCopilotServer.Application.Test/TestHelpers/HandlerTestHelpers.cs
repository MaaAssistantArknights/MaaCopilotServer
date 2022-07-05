// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;
using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestActivationToken;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
using MaaCopilotServer.Application.System.GetCurrentVersion;
using MaaCopilotServer.Application.System.SendEmailTest;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Resources;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.Test.TestHelpers;

/// <summary>
/// The helper class for testing handlers.
/// </summary>
[ExcludeFromCodeCoverage]
public class HandlerTest
{
    #region Test Constants
    /// <summary>
    /// The test username.
    /// </summary>
    public const string TestUsername = "test_username";

    /// <summary>
    /// The test password.
    /// </summary>
    public const string TestPassword = "test_password";

    /// <summary>
    /// The test hashed password.
    /// </summary>
    public const string TestHashedPassword = "hashed_password";

    /// <summary>
    /// The test email.
    /// </summary>
    public const string TestEmail = "user@example.com";

    /// <summary>
    /// The test token.
    /// </summary>
    public const string TestToken = "token";

    /// <summary>
    /// The test token expiration time.
    /// </summary>
    public static readonly DateTimeOffset TestTokenTime = new(2020, 1, 1, 0, 0, 0, default);

    /// <summary>
    /// The test token expiration time, which has expired.
    /// </summary>
    public static readonly DateTimeOffset TestTokenTimePast = new(1900, 1, 1, 0, 0, 0, default);

    /// <summary>
    /// The test token expiration time, which has not yet arrived.
    /// </summary>
    public static readonly DateTimeOffset TestTokenTimeFuture = new(9999, 12, 31, 23, 59, 59, default);
    #endregion

    #region Mocks
    /// <summary>
    /// The API error message.
    /// </summary>
    public ApiErrorMessage ApiErrorMessage { get; } = new();

    /// <summary>
    /// The validation error message.
    /// </summary>
    public ValidationErrorMessage ValidationErrorMessage { get; } = new();

    /// <summary>
    /// The domain string i18n resouece.
    /// </summary>
    public DomainString DomainString { get; } = new();

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
        EnableTestEmailApi = default,
        TestEmailApiToken = TestToken,
    });

    /// <summary>
    /// The operation options.
    /// </summary>
    public IOptions<CopilotOperationOption> CopilotOperationOption { get; private set; } = Options.Create(new CopilotOperationOption()
    {
        DislikeMultiplier = 2,
        LikeMultiplier = 10,
        ViewMultiplier = 1,
        RequireTitle = default,
        RequireDetails = default
    });

    /// <summary>
    /// The application options.
    /// </summary>
    public IOptions<ApplicationOption> ApplicationOption { get; private set; } = Options.Create(new ApplicationOption()
    {
        AssemblyPath = string.Empty,
        DataDirectory = string.Empty,
        Version = string.Empty,
    });
    #endregion

    #region Mock Setups
    /// <summary>
    /// Setups <see cref="DbContext"/>.
    /// </summary>
    /// <param name="action">The setup action.</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
    public HandlerTest SetupDatabase(Action<IMaaCopilotDbContext> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action.Invoke(DbContext);
        DbContext.SaveChangesAsync(new CancellationToken()).Wait();
        return this;
    }

    /// <summary>
    /// Setups <see cref="CurrentUserService"/>.
    /// </summary>
    /// <param name="action">The setup action.</param>
    /// <returns>The current instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
    public HandlerTest SetupCurrentUserService(Action<Mock<ICurrentUserService>> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
    public HandlerTest SetupSecretService(Action<Mock<ISecretService>> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is <c>null</c>.</exception>
    public HandlerTest SetupEmailService(Action<Mock<IMailService>> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action.Invoke(MailService);
        return this;
    }

    /// <summary>
    /// Setups <see cref="MailService"/>.
    /// </summary>
    /// <param name="mock">The new mock.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupEmailService(Mock<IMailService> mock)
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
    /// Setups <see cref="CopilotOperationOption"/>.
    /// </summary>
    /// <param name="newOption">The new option.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupCopilotOperationOption(CopilotOperationOption newOption)
    {
        CopilotOperationOption = Options.Create(newOption);
        return this;
    }

    /// <summary>
    /// Setups <see cref="CopilotOperationOption"/>.
    /// </summary>
    /// <param name="newOption">The new option.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupCopilotServerOption(CopilotServerOption newOption)
    {
        CopilotServerOption = Options.Create(newOption);
        return this;
    }

    /// <summary>
    /// Setups <see cref="ApplicationOption"/>.
    /// </summary>
    /// <param name="newOption">The new option.</param>
    /// <returns>The current instance for method chaining.</returns>
    public HandlerTest SetupApplicationOption(ApplicationOption newOption)
    {
        ApplicationOption = Options.Create(newOption);
        return this;
    }
    #endregion

    #region Testers
    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestCreateCopilotOperation(CreateCopilotOperationCommand request)
    {
        var handler = new CreateCopilotOperationCommandHandler(DbContext, CurrentUserService.Object, new CopilotOperationService(CopilotOperationOption, DomainString), CopilotOperationOption, ValidationErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestDeleteCopilotOperation(DeleteCopilotOperationCommand request)
    {
        var handler = new DeleteCopilotOperationCommandHandler(DbContext, new CopilotOperationService(CopilotOperationOption, DomainString), CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestGetCopilotOperation(GetCopilotOperationQuery request)
    {
        var handler = new GetCopilotOperationQueryHandler(DbContext, CurrentUserService.Object, new CopilotOperationService(CopilotOperationOption, DomainString), ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="QueryCopilotOperationsQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestQueryCopilotOperations(QueryCopilotOperationsQuery request)
    {
        var handler = new QueryCopilotOperationsQueryHandler(DbContext, new CopilotOperationService(CopilotOperationOption, DomainString), CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestChangeCopilotUserInfo(ChangeCopilotUserInfoCommand request)
    {
        var handler = new ChangeCopilotUserInfoCommandHandler(DbContext, CurrentUserService.Object, SecretService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestPasswordReset(PasswordResetCommand request)
    {
        var handler = new PasswordResetCommandHandler(SecretService.Object, DbContext, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestRequestPasswordReset(RequestPasswordResetCommand request)
    {
        var handler = new RequestPasswordResetCommandHandler(TokenOption, DbContext, SecretService.Object, MailService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestUpdateCopilotUserInfo(UpdateCopilotUserInfoCommand request)
    {
        var handler = new UpdateCopilotUserInfoCommandHandler(TokenOption, DbContext, MailService.Object, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestUpdateCopilotUserPassword(UpdateCopilotUserPasswordCommand request)
    {
        var handler = new UpdateCopilotUserPasswordCommandHandler(DbContext, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestActivateCopilotAccount(ActivateCopilotAccountCommand request)
    {
        var handler = new ActivateCopilotAccountCommandHandler(CurrentUserService.Object, DbContext, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestCreateCopilotUser(CreateCopilotUserCommand request)
    {
        var handler = new CreateCopilotUserCommandHandler(DbContext, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestRegisterCopilotAccount(RegisterCopilotAccountCommand request)
    {
        var handler = new RegisterCopilotAccountCommandHandler(TokenOption, CurrentUserService.Object, DbContext, SecretService.Object, MailService.Object, CopilotServerOption, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestDeleteCopilotUser(DeleteCopilotUserCommand request)
    {
        var handler = new DeleteCopilotUserCommandHandler(DbContext, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="LoginCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestLoginCopilotUser(LoginCopilotUserCommand request)
    {
        var handler = new LoginCopilotUserCommandHandler(DbContext, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestRequestActivationToken(RequestActivationTokenCommand request)
    {
        var handler = new RequestActivationTokenCommandHandler(TokenOption, DbContext, MailService.Object, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestGetCopilotUser(GetCopilotUserQuery request)
    {
        var handler = new GetCopilotUserQueryHandler(DbContext, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="QueryCopilotUserQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestQueryCopilotUser(QueryCopilotUserQuery request)
    {
        var handler = new QueryCopilotUserQueryHandler(DbContext, CurrentUserService.Object);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="GetCurrentVersionCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestGetCurrentVersion(GetCurrentVersionCommand request)
    {
        var handler = new GetCurrentVersionCommandHandler(ApplicationOption);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestSendEmailTest(SendEmailTestCommand request)
    {
        var handler = new SendEmailTestCommandHandler(MailService.Object, CopilotServerOption);
        return new HandlerTestResult { Response = handler.Handle(request, new CancellationToken()).GetAwaiter().GetResult(), DbContext = DbContext };
    }
    #endregion
}

/// <summary>
/// The handler test result.
/// </summary>
[ExcludeFromCodeCoverage]
public record HandlerTestResult
{
    /// <summary>
    /// The response.
    /// </summary>
    public MaaApiResponse Response { get; init; } = default!;

    /// <summary>
    /// The DB context.
    /// </summary>
    public IMaaCopilotDbContext DbContext { get; init; } = default!;
}

/// <summary>
/// The extension class for <see cref="HandlerTest"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class HandlerTestExtension
{
    #region Current User Service
    /// <summary>
    /// Setups <see cref="ICurrentUserService.GetUserIdentity"/>
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <param name="returns">The returned value.</param>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="test"/> is <c>null</c>.</exception>
    public static HandlerTest SetupGetUserIdentity(this HandlerTest test, Guid? returns)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        return test.SetupCurrentUserService(mock => mock.Setup(x => x.GetUserIdentity()).Returns(returns));
    }

    /// <summary>
    /// Setups <see cref="ICurrentUserService.GetUser"/>
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <param name="returns">The returned value.</param>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="test"/> is <c>null</c>.</exception>
    public static HandlerTest SetupGetUser(this HandlerTest test, Domain.Entities.CopilotUser? returns)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        return test.SetupCurrentUserService(mock => mock.Setup(x => x.GetUser().Result).Returns(returns));
    }
    #endregion

    #region Secret Service
    /// <summary>
    /// Setups <see cref="ISecretService.HashPassword(string)"/>.
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <param name="password">The test password.</param>
    /// <param name="returns">The returned value.</param>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="test"/> is <c>null</c>.</exception>
    public static HandlerTest SetupHashPassword(this HandlerTest test, string password = HandlerTest.TestPassword, string returns = HandlerTest.TestHashedPassword)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        return test.SetupSecretService(mock => mock.Setup(x => x.HashPassword(password)).Returns(returns));
    }

    /// <summary>
    /// Setups <see cref="ISecretService.VerifyPassword(string, string)"/>.
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <param name="hash">The test hash.</param>
    /// <param name="password">The test password.</param>
    /// <param name="result">The returned value.</param>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="test"/> is <c>null</c>.</exception>
    public static HandlerTest SetupVerifyPassword(this HandlerTest test, string hash, string password, bool result)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        return test.SetupSecretService(mock => mock.Setup(x => x.VerifyPassword(hash, password)).Returns(result));
    }

    /// <summary>
    /// Setups <see cref="ISecretService.GenerateToken(Guid, TimeSpan)"/>.
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <param name="returnedToken">The returned token.</param>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="test"/> is <c>null</c>.</exception>
    public static HandlerTest SetupGenerateToken(this HandlerTest test, string returnedToken = HandlerTest.TestToken)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        return test.SetupSecretService(mock => mock.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<TimeSpan>())).Returns((returnedToken, HandlerTest.TestTokenTime)));
    }

    /// <summary>
    /// Setups <see cref="ISecretService.GenerateJwtToken(Guid)"/>.
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <param name="userEntity">The test user entity.</param>
    /// <param name="returnedToken">The returned token.</param>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="test"/> is <c>null</c>.</exception>
    public static HandlerTest SetupGenerateJwtToken(this HandlerTest test, Guid userEntity, string returnedToken = HandlerTest.TestToken)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        return test.SetupSecretService(mock => mock.Setup(x => x.GenerateJwtToken(userEntity)).Returns((returnedToken, HandlerTest.TestTokenTime)));
    }
    #endregion

    #region Mail Service
    /// <summary>
    /// Setups <see cref="IMailService.SendEmailAsync{T}(T, string)"/>.
    /// </summary>
    /// <param name="test">The <see cref="HandlerTest"/> instance.</param>
    /// <param name="success">Whether the email sending is successful.</param>
    /// <param name="targetAddress">The test target address.</param>
    /// <returns>The <see cref="HandlerTest"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="test"/> is <c>null</c>.</exception>
    public static HandlerTest SetupSendEmailAsync(this HandlerTest test, bool success, string targetAddress = HandlerTest.TestEmail)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        return test.SetupEmailService(mock => mock.Setup(x => x.SendEmailAsync(It.IsAny<IEmailModel>(), targetAddress).Result).Returns(success));
    }
    #endregion
}