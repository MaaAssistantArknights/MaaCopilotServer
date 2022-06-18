// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;
using MaaCopilotServer.Api.Middleware;
using MaaCopilotServer.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MaaCopilotServer.Api.Test.Middleware;

/// <summary>
///     Tests for <see cref="RequestCultureMiddleware" />.
/// </summary>
[TestClass]
public class RequestCultureMiddlewareTest
{
    /// <summary>
    ///     Tests constructor.
    /// </summary>
    [TestMethod]
    public void TestConstructor()
    {
        var requestCultureMiddleware = new RequestCultureMiddleware(Substitute.For<RequestDelegate>());
        requestCultureMiddleware.Should().NotBeNull();
    }

    /// <summary>
    ///     Tests <see cref="RequestCultureMiddleware.InvokeAsync(HttpContext, ValidationErrorMessage, ApiErrorMessage)" />
    /// </summary>
    /// <param name="culture">The culture in the request query.</param>
    /// <param name="expectedCulture">The expected culture.</param>
    /// <returns>N/A</returns>
    [DataTestMethod]
    [DataRow("en", "en")]
    [DataRow("zh-cn", "zh-cn")]
    [DataRow(null, "zh-cn")] // default
    public async Task TestInvokeAsync(string? culture, string expectedCulture)
    {
        var requestCultureMiddleware = new RequestCultureMiddleware(_ => Task.CompletedTask);
        var context = Substitute.For<HttpContext>();
        context.Request.Returns(_ =>
        {
            var request = Substitute.For<HttpRequest>();
            request.Query.Returns(_ =>
            {
                var query = Substitute.For<IQueryCollection>();
                query
                    .TryGetValue("culture", out Arg.Any<StringValues>())
                    .Returns(c =>
                    {
                        c[1] = new StringValues(culture);
                        return culture != null;
                    });
                return query;
            });
            return request;
        });

        var validationErrorMessage = Substitute.For<ValidationErrorMessage>();
        var apiErrorMessage = Substitute.For<ApiErrorMessage>();

        await requestCultureMiddleware.InvokeAsync(context, validationErrorMessage, apiErrorMessage);
        validationErrorMessage.Received().CultureInfo.Should().BeEquivalentTo(new CultureInfo(expectedCulture));
        apiErrorMessage.Received().CultureInfo.Should().BeEquivalentTo(new CultureInfo(expectedCulture));
    }
}
