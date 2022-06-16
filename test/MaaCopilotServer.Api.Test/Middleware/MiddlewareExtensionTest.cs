// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Api.Test.Middleware
{
    using MaaCopilotServer.Api.Middleware;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Tests for <see cref="MiddlewareExtension"/>.
    /// </summary>
    [TestClass]
    public class MiddlewareExtensionTest
    {
        /// <summary>
        /// Tests <see cref="MiddlewareExtension.UseRequestCulture(IApplicationBuilder)"/>.
        /// </summary>
        [TestMethod]
        public void TestUseRequestCulture()
        {
            var app = Substitute.For<IApplicationBuilder>();
            app.UseRequestCulture();
            app.Received().UseRequestCulture();
        }
    }
}
