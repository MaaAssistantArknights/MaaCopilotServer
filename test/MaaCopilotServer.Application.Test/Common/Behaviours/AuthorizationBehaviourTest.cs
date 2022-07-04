// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
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
[ExcludeFromCodeCoverage]
public class AuthorizationBehaviourTest
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage = new();

    /// <summary>
    ///     The service of current testUser.
    /// </summary>
    private readonly ICurrentUserService _currentUserService = Mock.Of<ICurrentUserService>();

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with different roles.
    /// </summary>
    /// <param name="userRole">The role of the user.</param>
    /// <param name="requiredRole">The role required to access the endpoint.</param>
    /// <param name="expectedErrorStatusCode">The expected status code, or <c>null</c> if no exception should be thrown</param>
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
    public void TestHandle(UserRole userRole, UserRole requiredRole, int expectedErrorStatusCode)
    {
        IRequest<MaaApiResponse> testRequest = requiredRole switch
        {
            UserRole.User => new TestUserRole(),
            UserRole.Uploader => new TestUploaderRole(),
            UserRole.Admin => new TestAdminRole(),
            UserRole.SuperAdmin => new TestSuperAdminRole(),
            _ => throw new ArgumentOutOfRangeException(nameof(userRole))
        };
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(Guid.NewGuid());
        currentUserService.Setup(x => x.GetUser().Result).Returns(new Domain.Entities.CopilotUser(
            string.Empty, string.Empty, string.Empty, userRole, Guid.Empty));

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(currentUserService.Object, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));
        var response = action().GetAwaiter().GetResult();

        response.StatusCode.Should().Be(expectedErrorStatusCode);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     without <see cref="AuthorizedAttribute"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle_NoAttribute()
    {
        IRequest<MaaApiResponse> testRequest = new TestNoRole();

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(
            _currentUserService, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));

        var response = action().GetAwaiter().GetResult();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with null user ID.
    /// </summary>
    [TestMethod]
    public void TestHandle_NullUserId()
    {
        IRequest<MaaApiResponse> testRequest = new TestUserRole();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns((Guid?)null);

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(
            currentUserService.Object, _apiErrorMessage); ;
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));

        var response = action().GetAwaiter().GetResult();
        response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with null user.
    /// </summary>
    [TestMethod]
    public void TestHandle_NullUser()
    {
        IRequest<MaaApiResponse> testRequest = new TestUserRole();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(Guid.NewGuid());

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>
            (currentUserService.Object, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));

        var response = action().GetAwaiter().GetResult();
        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     with deactivated user.
    /// </summary>
    [TestMethod]
    public void TestHandle_DeactivatedUser()
    {
        IRequest<MaaApiResponse> testRequest = new TestUserRoleDeactivated();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUserIdentity()).Returns(Guid.NewGuid());
        currentUserService.Setup(x => x.GetUser().Result).Returns(new Domain.Entities.CopilotUser(
            string.Empty, string.Empty, string.Empty, UserRole.User, Guid.Empty));

        var behaviour = new AuthorizationBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(
            currentUserService.Object, _apiErrorMessage);
        var action = async () =>
            await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(MaaApiResponseHelper.Ok()));

        var response = action().GetAwaiter().GetResult();
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
