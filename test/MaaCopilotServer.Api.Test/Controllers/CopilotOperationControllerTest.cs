// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Api.Controllers;
using MaaCopilotServer.Api.Test.TestHelpers;

namespace MaaCopilotServer.Api.Test.Controllers;

/// <summary>
///     Tests <see cref="CopilotOperationController" />.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CopilotOperationControllerTest
{
    /// <summary>
    /// Tests actions of <see cref="CopilotOperationController"/>.
    /// </summary>
    [TestMethod]
    public void Test()
    {
        ControllerTestHelper.Test<CopilotOperationController>(c => c.CreateCopilotOperation(new()));
        ControllerTestHelper.Test<CopilotOperationController>(c => c.DeleteCopilotOperation(new()));
        ControllerTestHelper.Test<CopilotOperationController>(c => c.GetCopilotOperation(string.Empty, string.Empty));
        ControllerTestHelper.Test<CopilotOperationController>(c => c.QueryCopilotOperation(new()));
        ControllerTestHelper.Test<CopilotOperationController>(c => c.UpdateCopilotOperation(new()));
        ControllerTestHelper.Test<CopilotOperationController>(c => c.RatingCopilotOperation(new()));
    }
}
