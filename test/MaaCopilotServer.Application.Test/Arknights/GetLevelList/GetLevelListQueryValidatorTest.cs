// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Arknights.GetLevelList;

namespace MaaCopilotServer.Application.Test.Arknights.GetLevelList;

/// <summary>
/// Tests <see cref="GetLevelListQueryValidator"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class GetLevelListQueryValidatorTest
{
    /// <summary>
    /// Tests <see cref="GetLevelListQueryValidator"/>.
    /// </summary>
    /// <param name="server">The server name.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow("cn", true)]
    [DataRow("en", true)]
    [DataRow("ja", true)]
    [DataRow("ko", true)]
    [DataRow("??", false)]
    public void Test(string server, bool expected)
    {
        ValidatorTestHelper.Test<GetLevelListQueryValidator, GetLevelListQuery>(new()
        {
            Server = server,
        }, expected);
    }
}
