// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;

namespace MaaCopilotServer.Api.Test.Controllers;

/// <summary>
///     Tests <see cref="ArknightsController" />.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class ArknightsControllerTest
{
    /// <summary>
    /// Tests actions of <see cref="ArknightsController"/>.
    /// </summary>
    [TestMethod]
    public void Test()
    {
        ControllerTestHelper.Test<ArknightsController>(c => c.GetArkDataVersion());
        ControllerTestHelper.Test<ArknightsController>(c => c.GetArkLevelList(new()));
        ControllerTestHelper.Test<ArknightsController>(c => c.GetArkOperatorList(new()));
    }
}
