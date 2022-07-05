// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.System.GetCurrentVersion;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.System.GetCurrentVersion;

/// <summary>
/// Tests <see cref="GetCurrentVersionCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class GetCurrentVersionCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="GetCurrentVersionCommandHandler.Handle(GetCurrentVersionCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var response = new HandlerTest()
            .SetupApplicationOption(new()
            {
                Version = "0.0.1",
            })
            .TestGetCurrentVersion(new())
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        ((GetCurrentVersionDto)response.Data!).Version.Should().Be("0.0.1");
    }
}
