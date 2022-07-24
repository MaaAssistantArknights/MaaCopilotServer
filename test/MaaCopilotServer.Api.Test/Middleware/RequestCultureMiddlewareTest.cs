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
///     Tests <see cref="RequestCultureMiddleware" />.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class RequestCultureMiddlewareTest
{
    /// <summary>
    ///     Tests <see cref="RequestCultureMiddleware.InvokeAsync(HttpContext, ValidationErrorMessage, ApiErrorMessage)" />
    /// </summary>
    /// <param name="culture">The culture in the request query.</param>
    /// <param name="expectedCulture">The expected culture.</param>
    [DataTestMethod]
    [DataRow("en", "en")]
    [DataRow("zh-cn", "zh-cn")]
    [DataRow(null, "zh-cn")] // default
    public void TestInvokeAsync(string? culture, string expectedCulture)
    {
        var queryCollectionDict = new Dictionary<string, StringValues>();
        var itemsDict = new Dictionary<object, object?>();
        if (culture != null)
        {
            queryCollectionDict.Add("culture", new StringValues(culture));
        }
        var requestCultureMiddleware = new RequestCultureMiddleware(_ => Task.CompletedTask);
        var context = Mock.Of<HttpContext>(x =>
            x.Request.Query == new QueryCollection(queryCollectionDict) &&
            x.Items == itemsDict
        );

        requestCultureMiddleware.InvokeAsync(
            context,
            Mock.Of<ValidationErrorMessage>(),
            Mock.Of<ApiErrorMessage>(),
            Mock.Of<DomainString>()).Wait();
        itemsDict.Should().NotBeEmpty();
        itemsDict["culture"].Should().BeEquivalentTo(new CultureInfo(expectedCulture));
    }
}
