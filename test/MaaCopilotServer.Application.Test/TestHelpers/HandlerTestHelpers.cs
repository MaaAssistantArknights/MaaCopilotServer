// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Arknights.GetDataVersion;
using MaaCopilotServer.Application.Common.Interfaces;
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
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Domain.Options;
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
    /// The domain string i18n resource.
    /// </summary>
    public DomainString DomainString { get; } = new();

    /// <summary>
    /// The DB context.
    /// </summary>
    public IMaaCopilotDbContext DbContext { get; } = new TestDbContext();

    /// <summary>
    /// The copilot operation service.
    /// </summary>
    public Mock<ICopilotOperationService> CopilotOperationService { get; private set; } = new();

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
    /// The operation process service.
    /// </summary>
    public Mock<IOperationProcessService> OperationProcessService { get; private set; } = new();

    /// <summary>
    /// The token options.
    /// </summary>
    public TokenOption TokenOption { get; set; } = new()
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
    };

    /// <summary>
    /// The copilot server options.
    /// </summary>
    public CopilotServerOption CopilotServerOption { get; set; } = new()
    {
        RegisterUserDefaultRole = Domain.Enums.UserRole.User,
        EnableTestEmailApi = default,
        TestEmailApiToken = TestToken,
    };

    /// <summary>
    /// The operation options.
    /// </summary>
    public CopilotOperationOption CopilotOperationOption { get; set; } = new()
    {
        DislikeMultiplier = 2,
        LikeMultiplier = 10,
        ViewMultiplier = 1
    };

    /// <summary>
    /// The application options.
    /// </summary>
    public ApplicationOption ApplicationOption { get; set; } = new()
    {
        AssemblyPath = string.Empty,
        DataDirectory = string.Empty,
        Version = string.Empty,
    };
    #endregion

    #region Testers
    #region Arknights
    /// <summary>
    /// Tests <see cref="GetDataVersionQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestGetDataVersion(GetDataVersionQuery request)
    {
        var handler = new GetDataVersionQueryHandler(DbContext);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }
    #endregion

    #region CopilotOperation
    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestCreateCopilotOperation(CreateCopilotOperationCommand request)
    {
        var handler = new CreateCopilotOperationCommandHandler(DbContext, CurrentUserService.Object, OperationProcessService.Object, CopilotOperationService.Object);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestDeleteCopilotOperation(DeleteCopilotOperationCommand request)
    {
        var handler = new DeleteCopilotOperationCommandHandler(DbContext, CopilotOperationService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestGetCopilotOperation(GetCopilotOperationQuery request)
    {
        var handler = new GetCopilotOperationQueryHandler(DbContext, CurrentUserService.Object, CopilotOperationService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="QueryCopilotOperationsQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestQueryCopilotOperations(QueryCopilotOperationsQuery request)
    {
        var handler = new QueryCopilotOperationsQueryHandler(DbContext, CopilotOperationService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }
    #endregion

    #region CopilotUser
    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestChangeCopilotUserInfo(ChangeCopilotUserInfoCommand request)
    {
        var handler = new ChangeCopilotUserInfoCommandHandler(DbContext, CurrentUserService.Object, SecretService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestPasswordReset(PasswordResetCommand request)
    {
        var handler = new PasswordResetCommandHandler(SecretService.Object, DbContext, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestRequestPasswordReset(RequestPasswordResetCommand request)
    {
        var handler = new RequestPasswordResetCommandHandler(Options.Create(TokenOption), DbContext, SecretService.Object, MailService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestUpdateCopilotUserInfo(UpdateCopilotUserInfoCommand request)
    {
        var handler = new UpdateCopilotUserInfoCommandHandler(Options.Create(TokenOption), DbContext, MailService.Object, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestUpdateCopilotUserPassword(UpdateCopilotUserPasswordCommand request)
    {
        var handler = new UpdateCopilotUserPasswordCommandHandler(DbContext, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestActivateCopilotAccount(ActivateCopilotAccountCommand request)
    {
        var handler = new ActivateCopilotAccountCommandHandler(DbContext, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestCreateCopilotUser(CreateCopilotUserCommand request)
    {
        var handler = new CreateCopilotUserCommandHandler(DbContext, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestRegisterCopilotAccount(RegisterCopilotAccountCommand request)
    {
        var handler = new RegisterCopilotAccountCommandHandler(Options.Create(TokenOption), DbContext, SecretService.Object, MailService.Object, Options.Create(CopilotServerOption), ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestDeleteCopilotUser(DeleteCopilotUserCommand request)
    {
        var handler = new DeleteCopilotUserCommandHandler(DbContext, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="LoginCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestLoginCopilotUser(LoginCopilotUserCommand request)
    {
        var handler = new LoginCopilotUserCommandHandler(DbContext, SecretService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestRequestActivationToken(RequestActivationTokenCommand request)
    {
        var handler = new RequestActivationTokenCommandHandler(Options.Create(TokenOption), DbContext, MailService.Object, SecretService.Object, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestGetCopilotUser(GetCopilotUserQuery request)
    {
        var handler = new GetCopilotUserQueryHandler(DbContext, CurrentUserService.Object, ApiErrorMessage);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="QueryCopilotUserQueryHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestQueryCopilotUser(QueryCopilotUserQuery request)
    {
        var handler = new QueryCopilotUserQueryHandler(DbContext);
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }
    #endregion

    #region System
    /// <summary>
    /// Tests <see cref="GetCurrentVersionCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestGetCurrentVersion(GetCurrentVersionCommand request)
    {
        var handler = new GetCurrentVersionCommandHandler(Options.Create(ApplicationOption));
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }

    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler"/>.
    /// </summary>
    /// <param name="request">The test request.</param>
    /// <returns>The result.</returns>
    public HandlerTestResult TestSendEmailTest(SendEmailTestCommand request)
    {
        var handler = new SendEmailTestCommandHandler(MailService.Object, Options.Create(CopilotServerOption));
        return new HandlerTestResult { Response = handler.Handle(request, default).Result, DbContext = DbContext };
    }
    #endregion
    #endregion
}
