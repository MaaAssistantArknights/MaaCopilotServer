// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.


using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Attributes;
using Microsoft.Extensions.Configuration;

namespace MaaCopilotServer.Application.Test.Common.Extensions;

/// <summary>
///     Tests of <see cref="ConfigurationExtension" />.
/// </summary>
[TestClass]
public class ConfigurationExtensionTest
{
    /// <summary>
    ///     Tests <see cref="ConfigurationExtension.GetOption{T}(IConfiguration)" /> with valid
    ///     <see cref="OptionNameAttribute" />.
    /// </summary>
    [TestMethod]
    public void TestGetOption_WithOptionName()
    {
        var configurationMock = new Mock<IConfiguration>();
        configurationMock
            .Setup(x => x.GetSection("TestOption"))
            .Returns(new Mock<IConfigurationSection>().Object);
        var configuration = configurationMock.Object;

        var action = () => configuration.GetOption<TestOption>();

        action.Should().NotThrow();
    }

    /// <summary>
    ///     Tests <see cref="ConfigurationExtension.GetOption{T}(IConfiguration)" /> without <see cref="OptionNameAttribute" />
    ///     .
    /// </summary>
    [TestMethod]
    public void TestGetOption_WithoutOptionName()
    {
        var configuration = new Mock<IConfiguration>().Object;

        var action = () => configuration.GetOption<ConfigurationExtensionTest>();

        action.Should().Throw<ArgumentException>();
    }

    /// <summary>
    ///     The test class with <see cref="OptionNameAttribute" />.
    /// </summary>
    [OptionName("TestOption")]
    private class TestOption
    {
    }
}


