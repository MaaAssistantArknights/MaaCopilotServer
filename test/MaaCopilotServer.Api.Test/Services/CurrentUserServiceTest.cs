// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using System.Security.Claims;
using Elastic.Apm.Api;
using MaaCopilotServer.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute.ReturnsExtensions;

namespace MaaCopilotServer.Api.Test.Services;

/// <summary>
///     Tests for <see cref="CurrentUserService" />.
/// </summary>
[TestClass]
public class CurrentUserServiceTest
{
    /// <summary>
    ///     The mock configuration.
    /// </summary>
    private IConfiguration _configuration;

    /// <summary>
    ///     The mock HTTP context accessor.
    /// </summary>
    private IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _configuration = Substitute.For<IConfiguration>();
    }

    /// <summary>
    ///     Tests constructor.
    /// </summary>
    [TestMethod]
    public void TestConstructor()
    {
        new CurrentUserService(_httpContextAccessor, _configuration).Should().NotBeNull();
        new CurrentUserService(_httpContextAccessor, _configuration, () => Substitute.For<ITransaction>()).Should().NotBeNull();
    }

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
        _httpContextAccessor.HttpContext.Returns(_ =>
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
        var currentUserService = new CurrentUserService(_httpContextAccessor, _configuration);
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
    ///     Tests <see cref="CurrentUserService.GetUserIdentity" /> with null HTTP context.
    /// </summary>
    [TestMethod]
    public void TestGetUserIdentity_NullHttpContext()
    {
        _httpContextAccessor.HttpContext.ReturnsNull();
        var currentUserService = new CurrentUserService(_httpContextAccessor, _configuration);
        var userIdentity = currentUserService.GetUserIdentity();
        userIdentity.Should().BeNull();
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
        _httpContextAccessor.HttpContext.Returns(_ =>
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
        var testConfiguration = new Dictionary<string, string> { { "Switches:Apm", apmSwitch.ToString().ToLower() } };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration)
            .Build();

        CurrentTransactionProvider provider = () =>
        {
            if (apmIdentifier == null)
            {
                return null;
            }

            var transaction = Substitute.For<ITransaction>();
            transaction.TraceId.Returns(apmIdentifier);
            return transaction;
        };

        var currentUserService = new CurrentUserService(_httpContextAccessor,
            _configuration,
            provider);
        var trackingId = currentUserService.GetTrackingId();
        trackingId.Should().BeEquivalentTo(expected);
    }

    /// <summary>
    ///     Tests <see cref="CurrentUserService.GetTrackingId" /> with null HTTP context.
    /// </summary>
    /// <param name="apmSwitch">Indicates whether the APM feature is on/off.</param>
    /// <param name="apmIdentifier">The identifier in APM.</param>
    /// <param name="expected">The expected identifier.</param>
    [DataTestMethod]
    [DataRow(false, "test_apmIdentifier", "")]
    [DataRow(true, "test_apmIdentifier", "test_apmIdentifier")]
    [DataRow(true, null, "")]
    public void TestGetTrackingId_NullHttpContext(bool apmSwitch, string? apmIdentifier, string? expected)
    {
        _httpContextAccessor.HttpContext.ReturnsNull();
        var testConfiguration = new Dictionary<string, string> { { "Switches:Apm", apmSwitch.ToString().ToLower() } };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration)
            .Build();

        CurrentTransactionProvider provider = () =>
        {
            if (apmIdentifier == null)
            {
                return null;
            }

            var transaction = Substitute.For<ITransaction>();
            transaction.TraceId.Returns(apmIdentifier);
            return transaction;
        };

        var currentUserService = new CurrentUserService(_httpContextAccessor,
            _configuration,
            provider);
        var trackingId = currentUserService.GetTrackingId();
        trackingId.Should().BeEquivalentTo(expected);
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
        if (isNullContext)
        {
            _httpContextAccessor.HttpContext.ReturnsNull();
        }
        else
        {
            _httpContextAccessor.HttpContext.Returns(_ =>
            {
                var context = Substitute.For<HttpContext>();
                if (noValue)
                {
                    context.Items.Returns(new Dictionary<object, object?>());
                }
                else
                {
                    var returnedDict = new Dictionary<object, object?>();
                    if (isNullCultureInfo)
                    {
                        returnedDict.Add("culture", null);
                    }
                    else
                    {
                        if (isOtherType)
                        {
                            returnedDict.Add("culture", new object());
                        }
                        else
                        {
                            returnedDict.Add("culture", new CultureInfo(givenCulture));
                        }
                    }

                    context.Items.Returns(returnedDict);
                }

                return context;
            });
        }

        var currentUserService = new CurrentUserService(_httpContextAccessor, _configuration);
        currentUserService.GetCulture().Should().BeEquivalentTo(new CultureInfo(expected));
    }
}
