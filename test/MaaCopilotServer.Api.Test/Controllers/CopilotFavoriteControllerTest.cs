// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;
using MaaCopilotServer.Api.Test.TestHelpers;

namespace MaaCopilotServer.Api.Test.Controllers;

/// <summary>
///     Tests <see cref="CopilotFavoriteController" />.
/// </summary>
[TestClass]
public class CopilotFavoriteControllerTest
{
    /// <summary>
    /// Tests actions of <see cref="CopilotFavoriteController"/>.
    /// </summary>
    [TestMethod]
    public void Test()
    {
        ControllerTestHelper.Test<CopilotFavoriteController>(c => c.CreateFavoriteList(new()));
        ControllerTestHelper.Test<CopilotFavoriteController>(c => c.DeleteFavoriteList(new()));
        ControllerTestHelper.Test<CopilotFavoriteController>(c => c.AddFavoriteToList(new()));
        ControllerTestHelper.Test<CopilotFavoriteController>(c => c.RemoveFavoriteFromList(new()));
        ControllerTestHelper.Test<CopilotFavoriteController>(c => c.GetFavoriteList(string.Empty));
    }
}
