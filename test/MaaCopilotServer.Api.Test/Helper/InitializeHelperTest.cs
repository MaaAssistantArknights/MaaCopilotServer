// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.RegularExpressions;
using MaaCopilotServer.Api.Helper;
using Microsoft.Extensions.Configuration;

namespace MaaCopilotServer.Api.Test.Helper;

/// <summary>
/// Tests for <see cref="InitializeHelper"/>.
/// </summary>
[TestClass]
public class InitializeHelperTest
{
    /// <summary>
    /// Tests constructors of <see cref="InitializeHelper"/>.
    /// </summary>
    [TestMethod]
    public void TestConstructor()
    {
        new InitializeHelper(Substitute.For<IConfiguration>()).Should().NotBeNull();
        new InitializeHelper(Substitute.For<IConfiguration>(), Substitute.For<GlobalSettingsHelper>()).Should().NotBeNull();
    }

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
