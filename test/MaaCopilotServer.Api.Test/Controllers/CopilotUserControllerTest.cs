// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Controllers;

namespace MaaCopilotServer.Api.Test.Controllers;

/// <summary>
///     Tests <see cref="CopilotUserController" />.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CopilotUserControllerTest
{
    /// <summary>
    /// Tests actions of <see cref="CopilotUserController"/>.
    /// </summary>
    [TestMethod]
    public void Test()
    {
        ControllerTestHelper.Test<CopilotUserController>(c => c.ChangeCopilotUserInfo(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.CreateCopilotUser(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.DeleteCopilotUser(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.LoginCopilotUser(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.UpdateCopilotUserInfo(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.UpdateCopilotUserPassword(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.GetCopilotUser(string.Empty));
        ControllerTestHelper.Test<CopilotUserController>(c => c.QueryCopilotUser(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.RegisterAccount(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.ActivateAccount(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.RequestPasswordChange(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.PasswordChange(new()));
        ControllerTestHelper.Test<CopilotUserController>(c => c.RequestActivationCode(null));
        ControllerTestHelper.Test<CopilotUserController>(c => c.RequestActivationCode(new()));
    }
}
