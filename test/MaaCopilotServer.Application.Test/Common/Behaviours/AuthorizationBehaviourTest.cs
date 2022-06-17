// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Test.Common.Behaviours
{
    using Domain.Entities;
    using MediatR;
    using MaaCopilotServer.Application.Common.Behaviours;
    using MaaCopilotServer.Application.Common.Interfaces;
    using MaaCopilotServer.Resources;
    using MaaCopilotServer.Application.Common.Security;
    using MaaCopilotServer.Domain.Enums;
    using MaaCopilotServer.Application.Common.Models;
    using MaaCopilotServer.Application.Common.Exceptions;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Tests for <see cref="AuthorizationBehaviour{TRequest, TResponse}"/>
    /// </summary>
    [TestClass]
    public class AuthorizationBehaviourTest
    {
        /// <summary>
        /// The service of current testUser.
        /// </summary>
        private ICurrentUserService _currentUserService;

        /// <summary>
        /// The API error message.
        /// </summary>
        private ApiErrorMessage _apiErrorMessage;

        /// <summary>
        /// The service of identity.
        /// </summary>
        private IIdentityService _identityService;

        /// <summary>
        /// Initializes tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this._currentUserService = Substitute.For<ICurrentUserService>();
            this._apiErrorMessage = Substitute.For<ApiErrorMessage>();
            this._identityService = Substitute.For<IIdentityService>();
        }

        /// <summary>
        /// Tests <see cref="AuthorizationBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})"/>.
        /// </summary>
        /// <param name="isNullUserId">Indicates whether the user ID should be invalid.</param>
        /// <param name="isNullUser">Indicates whether the user info should be invalid.</param>
        /// <param name="userRole">The role of the user.</param>
        /// <param name="requiredRole">The role required to access the endpoint.</param>
        /// <param name="expectedErrorStatusCode">The expected status code, or <c>null</c> if no exception should be thrown</param>
        /// <returns>N/A</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="userRole"/> is not a valid value of <see cref="UserRole"/>.
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
            IRequest<object> testRequest = requiredRole switch
            {
                UserRole.User => new TestUserRole(),
                UserRole.Uploader => new TestUploaderRole(),
                UserRole.Admin => new TestAdminRole(),
                UserRole.SuperAdmin => new TestSuperAdminRole(),
                _ => throw new ArgumentOutOfRangeException(nameof(userRole)),
            };
            if (isNullUserId)
            {
                this._currentUserService.GetUserIdentity().Returns((Guid?)null);
            }
            else
            {
                this._currentUserService.GetUserIdentity().Returns(Guid.NewGuid());
            }
            if (isNullUser)
            {
                this._identityService
                    .GetUserAsync(Arg.Any<Guid>())
                    .ReturnsForAnyArgs((CopilotUser)null);
            }
            else
            {
                this._identityService
                    .GetUserAsync(Arg.Any<Guid>())
                    .ReturnsForAnyArgs(new CopilotUser(default, default, default, userRole, default));
            }
            var behaviour = new AuthorizationBehaviour<IRequest<object>, object>(this._identityService,
                                                                                 this._currentUserService,
                                                                                 this._apiErrorMessage);
            var action = async () => await behaviour.Handle(testRequest, new CancellationToken(), () => Task.FromResult(new object()));
            if (expectedErrorStatusCode != null)
            {
                await action.Should()
                            .ThrowAsync<PipelineException>()
                            .Where(e => e.Result.RealStatusCode == expectedErrorStatusCode);
            }
            else
            {
                await action.Should().NotThrowAsync();
            }
        }

        /// <summary>
        /// A test class with <see cref="UserRole.User"/> role for authorization testing.
        /// </summary>
        [Authorized(UserRole.User)]
        class TestUserRole : IRequest<object>
        {
        }

        /// <summary>
        /// A test class with <see cref="UserRole.Uploader"/> role for authorization testing.
        /// </summary>
        [Authorized(UserRole.Uploader)]
        class TestUploaderRole : IRequest<object>
        {
        }

        /// <summary>
        /// A test class with <see cref="UserRole.Admin"/> role for authorization testing.
        /// </summary>
        [Authorized(UserRole.Admin)]
        class TestAdminRole : IRequest<object>
        {
        }

        /// <summary>
        /// A test class with <see cref="UserRole.UploaSuperAdminder"/> role for authorization testing.
        /// </summary>
        [Authorized(UserRole.SuperAdmin)]
        class TestSuperAdminRole : IRequest<object>
        {
        }
    }
}