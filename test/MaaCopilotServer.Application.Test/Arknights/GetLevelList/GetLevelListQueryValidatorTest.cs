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
    /// <param name="language">The server language.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow("cn", true)]
    [DataRow("tw", true)]
    [DataRow("en", true)]
    [DataRow("ja", true)]
    [DataRow("ko", true)]
    [DataRow("zh_cn", true)]
    [DataRow("zh_tw", true)]
    [DataRow("en_us", true)]
    [DataRow("ja_jp", true)]
    [DataRow("ko_kr", true)]
    [DataRow("", true)]
    [DataRow("??", false)]
    public void Test(string language, bool expected)
    {
        ValidatorTestHelper.Test<GetLevelListQueryValidator, GetLevelListQuery>(new()
        {
            Language = language,
        }, expected);
    }
}
