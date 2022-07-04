// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Queries.GetCopilotUser;

/// <summary>
/// Tests <see cref="GetCopilotUserQueryValidator"/>.
/// </summary>
[TestClass]
public class GetCopilotUserQueryValidatorTest
{
    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryValidator"/>.
    /// </summary>
    /// <param name="userId">The test user ID.</param>
    /// <param name="expected">The expected validation result.</param>
    [DataTestMethod]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", true)]
    [DataRow("me", true)]
    [DataRow(null, false)]
    [DataRow("invalid_guid", false)]
    public void Test(string? userId, bool expected)
    {
        ValidatorTestHelper.Test<GetCopilotUserQueryValidator, GetCopilotUserQuery>(new()
        {
            UserId = userId
        }, expected);
    }
}
