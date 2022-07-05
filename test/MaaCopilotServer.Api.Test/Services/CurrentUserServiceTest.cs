// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Claims;
using MaaCopilotServer.Api.Services;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MaaCopilotServer.Api.Test.Services;

/// <summary>
///     Tests <see cref="CurrentUserService" />.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CurrentUserServiceTest
{
    /// <summary>
    ///     The mock configuration.
    /// </summary>
    private readonly IConfiguration _configuration = Mock.Of<IConfiguration>();

    /// <summary>
    /// The DB Context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();

    /// <summary>
    ///     Tests <see cref="CurrentUserService.GetUserIdentity" />.
    /// </summary>
    /// <param name="id">The user ID in the <see cref="Claim" />, or <c>null</c> if it does not exist.</param>
    /// <param name="expected">The expected GUID.</param>
    [DataTestMethod]
    [DataRow("123e4567-e89b-12d3-a456-426614174000", "123e4567-e89b-12d3-a456-426614174000")]
    [DataRow("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
    [DataRow("not_a_valid_guid", null)]
    [DataRow(null, null)]
    public void TestGetUserIdentity(string? id, string? expected)
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(() =>
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.User).Returns(() =>
            {
                var claims = new List<Claim>();
                if (id != null)
                {
                    claims.Add(new Claim("id", id));
                }

                var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
                return user;
            });
            return httpContext.Object;
        });
        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, _configuration);
        var userIdentity = currentUserService.GetUserIdentity();
        if (expected != null)
        {
            userIdentity.Should().NotBeNull();
            userIdentity!.Value.ToString().Should().Be(expected);
        }
        else
        {
            userIdentity.Should().BeNull();
        }
    }

    /// <summary>
    ///     Tests <see cref="CurrentUserService.GetUserIdentity" /> with null HTTP context.
    /// </summary>
    [TestMethod]
    public void TestGetUserIdentityNullHttpContext()
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext?)null);
        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, _configuration);
        var userIdentity = currentUserService.GetUserIdentity();
        userIdentity.Should().BeNull();
    }

    /// <summary>
    /// Tests <see cref="CurrentUserService.GetUser"/>.
    /// </summary>
    [TestMethod]
    public void TestGetUser()
    {
        var entity = new Domain.Entities.CopilotUser(
            string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        _dbContext.CopilotUsers.Add(entity);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var testId = entity.EntityId;
        testId.Should().NotBe(Guid.Empty);
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(() =>
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.User).Returns(() =>
            {
                var claims = new List<Claim>
                {
                    new Claim("id", testId.ToString())
                };
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
                return user;
            });
            return httpContext.Object;
        });

        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, _configuration);
        var user = currentUserService.GetUser().GetAwaiter().GetResult();

        user.Should().NotBeNull();
        user!.EntityId.Should().Be(testId);
    }

    /// <summary>
    /// Tests <see cref="CurrentUserService.GetUser"/> with invalid identity.
    /// </summary>
    [TestMethod]
    public void TestGetUserInvalidIdentity()
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(() =>
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.User).Returns(() =>
            {
                var claims = new List<Claim>
                {
                    new Claim("id", "")
                };
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
                return user;
            });
            return httpContext.Object;
        });

        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, _configuration);
        var user = currentUserService.GetUser().GetAwaiter().GetResult();

        user.Should().BeNull();
    }

    /// <summary>
    ///     Tests <see cref="CurrentUserService.GetTrackingId" />
    /// </summary>
    /// <param name="apmSwitch">Indicates whether the APM feature is on/off.</param>
    /// <param name="contextIdentifier">The identifier in HTTP context.</param>
    /// <param name="apmIdentifier">The identifier in APM.</param>
    /// <param name="expected">The expected identifier.</param>
    [DataTestMethod]
    [DataRow(false, "test_contextIdentifier", "test_apmIdentifier", "test_contextIdentifier")]
    [DataRow(false, null, "test_apmIdentifier", "")]
    [DataRow(true, "test_contextIdentifier", "test_apmIdentifier", "test_apmIdentifier")]
    [DataRow(true, "test_contextIdentifier", null, "test_contextIdentifier")]
    [DataRow(true, null, null, "")]
    public void TestGetTrackingId(bool apmSwitch, string? contextIdentifier, string? apmIdentifier, string? expected)
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(() =>
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.TraceIdentifier).Returns(contextIdentifier!);

            var items = new Dictionary<object, object?>();
            if (apmIdentifier != null)
            {
                items.Add("ApmTraceId", apmIdentifier);
            }
            httpContext.Setup(x => x.Items).Returns(items);

            return httpContext.Object;
        });
        var testConfiguration = new Dictionary<string, string> { { "Switches:Apm", apmSwitch.ToString().ToLower(CultureInfo.CurrentCulture) } };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration)
            .Build();

        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, configuration);
        var trackingId = currentUserService.GetTrackingId();
        trackingId.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    ///     Tests <see cref="CurrentUserService.GetTrackingId" /> with null APM trace ID.
    /// </summary>
    /// <param name="apmSwitch">Indicates whether the APM feature is on/off.</param>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestGetTrackingIdWithNullApmTraceId(bool apmSwitch)
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(() =>
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.TraceIdentifier).Returns("test_contextIdentifier");
            httpContext.Setup(x => x.Items).Returns(
                new Dictionary<object, object?>()
                {
                    {"ApmTraceId", null }
                });
            return httpContext.Object;
        });
        var testConfiguration = new Dictionary<string, string>
        {
            { "Switches:Apm", apmSwitch.ToString().ToLower(CultureInfo.CurrentCulture) }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration)
            .Build();

        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, configuration);
        var trackingId = currentUserService.GetTrackingId();
        trackingId.Should().BeEquivalentTo("test_contextIdentifier");
    }

    /// <summary>
    ///     Tests <see cref="CurrentUserService.GetTrackingId" /> with null HTTP context.
    /// </summary>
    /// <param name="apmSwitch">Indicates whether the APM feature is on/off.</param>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestGetTrackingIdNullHttpContext(bool apmSwitch)
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var testConfiguration = new Dictionary<string, string>
        {
            { "Switches:Apm", apmSwitch.ToString().ToLower(CultureInfo.CurrentCulture) }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration)
            .Build();

        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, configuration);
        var trackingId = currentUserService.GetTrackingId();
        trackingId.Should().BeEquivalentTo(string.Empty);
    }

    /// <summary>
    ///     Tests <see cref="CurrentUserService.GetCulture" />.
    /// </summary>
    /// <param name="isNullContext">Indicates whether the HTTP context is null.</param>
    /// <param name="noValue">Indicates whether the items of the context contains <c>"culture"</c> key.</param>
    /// <param name="isNullCultureInfo">Indicates whether the culture info is null.</param>
    /// <param name="isOtherType">
    ///     Indicates whether the value of the key is of other type instead of <see cref="CultureInfo" />
    ///     .
    /// </param>
    /// <param name="givenCulture">The culture string given.</param>
    /// <param name="expected">The expected culture result.</param>
    [DataTestMethod]
    [DataRow(false, false, false, false, "zh-cn", "zh-cn")]
    [DataRow(false, false, false, false, "en", "en")]
    [DataRow(true, false, false, false, "zh-cn", "zh-cn")]
    [DataRow(false, true, false, false, "zh-cn", "zh-cn")]
    [DataRow(false, false, true, false, "zh-cn", "zh-cn")]
    [DataRow(false, false, false, true, "zh-cn", "zh-cn")]
    public void TestGetCulture(bool isNullContext,
        bool noValue,
        bool isNullCultureInfo,
        bool isOtherType,
        string givenCulture,
        string expected)
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        if (isNullContext)
        {
            httpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext?)null);
        }
        else
        {
            httpContextAccessor.Setup(x => x.HttpContext).Returns(() =>
            {
                var context = new Mock<HttpContext>();
                var items = new Dictionary<object, object?>();
                if (!noValue)
                {
                    if (isNullCultureInfo)
                    {
                        items.Add("culture", null);
                    }
                    else
                    {
                        if (isOtherType)
                        {
                            items.Add("culture", new object());
                        }
                        else
                        {
                            items.Add("culture", new CultureInfo(givenCulture));
                        }
                    }
                }
                context.Setup(x => x.Items).Returns(items);
                return context.Object;
            });
        }

        var currentUserService = new CurrentUserService(_dbContext, httpContextAccessor.Object, _configuration);
        currentUserService.GetCulture().Should().BeEquivalentTo(new CultureInfo(expected));
    }
}
