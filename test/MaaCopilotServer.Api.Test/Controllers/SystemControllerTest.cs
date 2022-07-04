// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;
using MaaCopilotServer.Api.Test.TestHelpers;
using MaaCopilotServer.Domain.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Api.Test.Controllers;

/// <summary>
///     Tests for <see cref="SystemController" />.
/// </summary>
[TestClass]
public class SystemControllerTest
{
    /// <summary>
    /// Tests actions of <see cref="SystemController"/>.
    /// </summary>
    [TestMethod]
    public void Test()
    {
        var options = Options.Create(new CopilotServerOption()
        {
            EnableTestEmailApi = true,
        });
        ControllerTestHelper.Test(m => new SystemController(m, options), c => c.GetVersion());
        ControllerTestHelper.Test(m => new SystemController(m, options), c => c.SendEmailTest(new()));
    }

    /// <summary>
    /// Tests <see cref="SystemController.SendEmailTest(Application.System.SendEmailTest.SendEmailTestCommand)"/>
    /// with feature disabled.
    /// </summary>
    [TestMethod]
    public void TestSendEmailTest_Disabled()
    {
        var options = Options.Create(new CopilotServerOption()
        {
            EnableTestEmailApi = false,
        });
        var mediator = new Mock<IMediator>();

        new SystemController(mediator.Object, options).SendEmailTest(new()).GetAwaiter().GetResult().Should().BeEquivalentTo(new NotFoundResult());
    }
}
