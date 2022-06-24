// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Security;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Resources;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.Common.Behaviours;

/// <summary>
///     Tests for <see cref="AuthorizationBehaviour{TRequest,TResponse}" />.
/// </summary>
[TestClass]
public class AuthorizationBehaviourTest
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private ApiErrorMessage _apiErrorMessage;

    /// <summary>
    ///     The service of current testUser.
    /// </summary>
    private ICurrentUserService _currentUserService;

    /// <summary>
    ///     The service of identity.
    /// </summary>
    private IIdentityService _identityService;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _currentUserService = Substitute.For<ICurrentUserService>();
        _apiErrorMessage = Substitute.For<ApiErrorMessage>();
        _identityService = Substitute.For<IIdentityService>();
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     .
    /// </summary>
    /// <param name="isNullUserId">Indicates whether the user ID should be invalid.</param>
    /// <param name="isNullUser">Indicates whether the user info should be invalid.</param>
    /// <param name="userRole">The role of the user.</param>
    /// <param name="requiredRole">The role required to access the endpoint.</param>
    /// <param name="expectedErrorStatusCode">The expected status code, or <c>null</c> if no exception should be thrown</param>
    /// <returns>N/A</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="userRole" /> is not a valid value of <see cref="UserRole" />.
    /// </exception>
    [DataTestMethod]
    [DataRow(false, false, UserRole.Admin, UserRole.Admin, null)]
    [DataRow(true, false, UserRole.Admin, UserRole.Admin, StatusCodes.Status401Unauthorized)]
    [DataRow(false, true, UserRole.Admin, UserRole.Admin, StatusCodes.Status404NotFound)]
    [DataRow(false, false, UserRole.User, UserRole.Admin, StatusCodes.Status403Forbidden)]
    public async Task TestHandle(bool isNullUserId,
        bool isNullUser,
        UserRole userRole,
        UserRole requiredRole,
        int? expectedErrorStatusCode)
    {
        IRequest<MaaApiResponse> testRequest = requiredRole switch
        {
            UserRole.User => new TestUserRole(),
            UserRole.Uploader => new TestUploaderRole(),
            UserRole.Admin => new TestAdminRole(),
            UserRole.SuperAdmin => new TestSuperAdminRole(),
            _ => throw new ArgumentOutOfRangeException(nameof(userRole))
        };
        if (isNullUserId)
        {
            _currentUserService.GetUserIdentity().Returns((Guid?)null);
        }
        else
        {
            _currentUserService.GetUserIdentity().Returns(Guid.NewGuid());
        }

        if (isNullUser)
        {
            _identityService
                .GetUserAsync(Arg.Any<Guid>())
                .ReturnsForAnyArgs((Domain.Entities.CopilotUser)null);
        }
        else
        {
            _identityService
                .GetUserAsync(Arg.Any<Guid>())
                .ReturnsForAnyArgs(new Domain.Entities.CopilotUser(default, default, default, userRole, default));
        }

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_identityService,
            _currentUserService,
            _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok(null, string.Empty)));
        if (expectedErrorStatusCode != null)
        {
            var response = await action();
            response.StatusCode.Should().Be(expectedErrorStatusCode);
        }
        else
        {
            await action.Should().NotThrowAsync();
        }
    }

    /// <summary>
    ///     A test class with <see cref="UserRole.User" /> role for authorization testing.
    /// </summary>
    [Authorized(UserRole.User, true)]
    private class TestUserRole : IRequest<MaaApiResponse>
    {
    }

    /// <summary>
    ///     A test class with <see cref="UserRole.Uploader" /> role for authorization testing.
    /// </summary>
    [Authorized(UserRole.Uploader, true)]
    private class TestUploaderRole : IRequest<MaaApiResponse>
    {
    }

    /// <summary>
    ///     A test class with <see cref="UserRole.Admin" /> role for authorization testing.
    /// </summary>
    [Authorized(UserRole.Admin, true)]
    private class TestAdminRole : IRequest<MaaApiResponse>
    {
    }

    /// <summary>
    ///     A test class with <see cref="UserRole.UploaSuperAdminder" /> role for authorization testing.
    /// </summary>
    [Authorized(UserRole.SuperAdmin, true)]
    private class TestSuperAdminRole : IRequest<MaaApiResponse>
    {
    }
}
