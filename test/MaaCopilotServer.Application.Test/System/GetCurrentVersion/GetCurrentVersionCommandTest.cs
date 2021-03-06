// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.System.GetCurrentVersion;
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
        var test = new HandlerTest
        {
            ApplicationOption = new()
            {
                Version = "0.0.1",
            }
        };

        var result = test.TestGetCurrentVersion(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        ((GetCurrentVersionDto)result
            .Response.Data!).Version.Should().Be("0.0.1");
    }
}
