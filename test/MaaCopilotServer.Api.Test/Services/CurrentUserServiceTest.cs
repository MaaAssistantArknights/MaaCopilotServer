// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Api.Test.Services
{
    using System.Security.Claims;
    using MaaCopilotServer.Api.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using NSubstitute.ReturnsExtensions;

    /// <summary>
    /// Tests for <see cref="CurrentUserService"/>.
    /// </summary>
    [TestClass]
    public class CurrentUserServiceTest
    {
        /// <summary>
        /// The mock HTTP context accessor.
        /// </summary>
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mock configuration.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// Initializes tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this._httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            this._configuration = Substitute.For<IConfiguration>();
        }

        /// <summary>
        /// Tests constructor.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var currentUserService = new CurrentUserService(this._httpContextAccessor, this._configuration);
            currentUserService.Should().NotBeNull();
        }

        /// <summary>
        /// Tests <see cref="CurrentUserService.GetUserIdentity"/>.
        /// </summary>
        /// <param name="id">The user ID in the <see cref="Claim"/>, or <c>null</c> if it does not exist.</param>
        /// <param name="expected">The expected GUID.</param>
        [DataTestMethod]
        [DataRow("123e4567-e89b-12d3-a456-426614174000", "123e4567-e89b-12d3-a456-426614174000")]
        [DataRow("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        [DataRow("not_a_valid_guid", null)]
        [DataRow(null, null)]
        public void TestGetUserIdentity(string? id, string? expected)
        {
            this._httpContextAccessor.HttpContext.Returns(_ =>
            {
                var httpContext = Substitute.For<HttpContext>();
                httpContext.User.Returns(_ =>
                {
                    var claims = new List<Claim>();
                    if (id != null)
                    {
                        claims.Add(new Claim("id", id));
                    }
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
                    return user;
                });
                return httpContext;
            });
            var currentUserService = new CurrentUserService(this._httpContextAccessor, this._configuration);
            var userIdentity = currentUserService.GetUserIdentity();
            if (expected != null)
            {
                userIdentity.Should().NotBeNull();
                userIdentity.Value.ToString().Should().Be(expected);
            }
            else
            {
                userIdentity.Should().BeNull();
            }
        }

        /// <summary>
        /// Tests <see cref="CurrentUserService.GetTrackingId"/>
        /// </summary>
        /// <param name="apmSwitch">Indicates whether the APM feature is on/off.</param>
        /// <param name="contextIdentifier">The identifier in HTTP context.</param>
        /// <param name="apmIdentifier">The identifier in APM.</param>
        /// <param name="expected">The expected identifier.</param>
        [DataTestMethod]
        [DataRow(false, "test_contextIdentifier", "test_apmIdentifier", "test_contextIdentifier")]
        [DataRow(false, null, "test_apmIdentifier", "")]
        public void TestGetTrackingId(bool apmSwitch, string? contextIdentifier, string? apmIdentifier, string? expected)
        {
            this._httpContextAccessor.HttpContext.Returns(_ =>
            {
                var httpContext = Substitute.For<HttpContext>();
                if (contextIdentifier != null)
                {
                    httpContext.TraceIdentifier.Returns(contextIdentifier);
                }
                else
                {
                    httpContext.TraceIdentifier.ReturnsNull();
                }
                return httpContext;
            });
            var testConfiguration = new Dictionary<string, string>
            {
                { "Switches:Apm", apmSwitch.ToString().ToLower() },
            };
            this._configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfiguration)
                .Build();

            // TODO: refactor implementation to make APM testing possible.
            var currentUserService = new CurrentUserService(this._httpContextAccessor, this._configuration);
            var trackingId = currentUserService.GetTrackingId();
            trackingId.Should().BeEquivalentTo(expected);
        }
    }
}
