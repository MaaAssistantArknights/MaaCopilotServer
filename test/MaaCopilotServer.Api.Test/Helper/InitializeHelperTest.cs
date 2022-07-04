// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.RegularExpressions;
using MaaCopilotServer.Api.Helper;

namespace MaaCopilotServer.Api.Test.Helper;

/// <summary>
/// Tests <see cref="InitializeHelper"/>.
/// </summary>
[TestClass]
public class InitializeHelperTest
{
    /// <summary>
    /// Tests <see cref="InitializeHelper.GeneratePassword"/>.
    /// </summary>
    [TestMethod]
    public void TestGeneratePassword()
    {
        var pattern = new Regex(@"^[A-Z]{16}$");
        pattern.Match(InitializeHelper.GeneratePassword()).Success.Should().BeTrue();
    }
}
