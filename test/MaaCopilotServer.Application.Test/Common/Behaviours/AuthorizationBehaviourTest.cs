// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;

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
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _currentUserService = Substitute.For<ICurrentUserService>();
        _apiErrorMessage = Substitute.For<ApiErrorMessage>();
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with different roles.
    /// </summary>
    /// <param name="userRole">The role of the user.</param>
    /// <param name="requiredRole">The role required to access the endpoint.</param>
    /// <param name="expectedErrorStatusCode">The expected status code, or <c>null</c> if no exception should be thrown</param>
    /// <returns>N/A</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="userRole" /> is not a valid value of <see cref="UserRole" />.
    /// </exception>
    [DataTestMethod]
    [DataRow(UserRole.User, UserRole.Uploader, StatusCodes.Status403Forbidden)]
    [DataRow(UserRole.Uploader, UserRole.Admin, StatusCodes.Status403Forbidden)]
    [DataRow(UserRole.Admin, UserRole.SuperAdmin, StatusCodes.Status403Forbidden)]
    [DataRow(UserRole.User, UserRole.User, StatusCodes.Status200OK)]
    [DataRow(UserRole.Uploader, UserRole.User, StatusCodes.Status200OK)]
    [DataRow(UserRole.Admin, UserRole.User, StatusCodes.Status200OK)]
    [DataRow(UserRole.SuperAdmin, UserRole.User, StatusCodes.Status200OK)]
    public async Task TestHandle(
        UserRole userRole,
        UserRole requiredRole,
        int expectedErrorStatusCode)
    {
        IRequest<MaaApiResponse> testRequest = requiredRole switch
        {
            UserRole.User => new TestUserRole(),
            UserRole.Uploader => new TestUploaderRole(),
            UserRole.Admin => new TestAdminRole(),
            UserRole.SuperAdmin => new TestSuperAdminRole(),
            _ => throw new ArgumentOutOfRangeException(nameof(userRole))
        };
        _currentUserService.GetUserIdentity().Returns(Guid.NewGuid());
        _currentUserService.GetUser()
            .ReturnsForAnyArgs(new Domain.Entities.CopilotUser(default, default, default, userRole, default));

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_currentUserService, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));
        var response = await action();
        response.StatusCode.Should().Be(expectedErrorStatusCode);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     without <see cref="AuthorizedAttribute"/>.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_NoAttribute()
    {
        IRequest<MaaApiResponse> testRequest = new TestNoRole();

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_currentUserService, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));
        var response = await action();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with null user ID.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_NullUserId()
    {
        IRequest<MaaApiResponse> testRequest = new TestUserRole();
        _currentUserService.GetUserIdentity().Returns((Guid?)null);

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_currentUserService, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));
        var response = await action();
        response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with null user.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_NullUser()
    {
        IRequest<MaaApiResponse> testRequest = new TestUserRole();
        _currentUserService.GetUserIdentity().Returns(Guid.NewGuid());
        _currentUserService.GetUser()
            .Returns(Task.FromResult<Domain.Entities.CopilotUser?>(null));

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_currentUserService, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));
        var response = await action();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with deactivated user.
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle_DeactivatedUser()
    {
        IRequest<MaaApiResponse> testRequest = new TestUserRoleDeactivated();
        _currentUserService.GetUserIdentity().Returns(Guid.NewGuid());
        _currentUserService.GetUser()
            .ReturnsForAnyArgs(new Domain.Entities.CopilotUser(default, default, default, default, default));

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_currentUserService, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));
        var response = await action();
        response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    ///     A test class without authorization.
    /// </summary>
    private class TestNoRole : IRequest<MaaApiResponse>
    {
    }

    /// <summary>
    ///     A test class with <see cref="UserRole.User" /> role for authorization testing.
    /// </summary>
    [Authorized(UserRole.User, true)]
    private class TestUserRole : IRequest<MaaApiResponse>
    {
    }

    /// <summary>
    ///     A test class with <see cref="UserRole.User" /> role for authorization testing.
    ///     The API does not allow access from deactivated users.
    /// </summary>
    [Authorized(UserRole.User, false)]
    private class TestUserRoleDeactivated : IRequest<MaaApiResponse>
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
