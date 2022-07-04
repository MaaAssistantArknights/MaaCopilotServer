// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Infrastructure.Services;

namespace MaaCopilotServer.Infrastructure.Test.Services;

/// <summary>
/// Tests <see cref="CopilotIdService"/>.
/// </summary>
[TestClass]
public class CopilotIdServiceTest
{
    /// <summary>
    /// Tests <see cref="CopilotIdService.EncodeId(long)"/>.
    /// </summary>
    [TestMethod]
    public void TestEncodeId()
    {
        new CopilotIdService().EncodeId(42).Should().Be("10042");
    }

    /// <summary>
    /// Tests <see cref="CopilotIdService.DecodeId(string)"/> with invalid ID.
    /// </summary>
    [TestMethod]
    public void TestDecodeId_Invalid()
    {
        new CopilotIdService().DecodeId("invalid").Should().BeNull();
    }

    /// <summary>
    /// Tests <see cref="CopilotIdService.DecodeId(string)"/> with ID out of range.
    /// </summary>
    [TestMethod]
    public void TestDecodeId_OutOfRange()
    {
        new CopilotIdService().DecodeId("9999").Should().BeNull();
    }

    /// <summary>
    /// Tests <see cref="CopilotIdService.DecodeId(string)"/>.
    /// </summary>
    [TestMethod]
    public void TestDecodeId()
    {
        new CopilotIdService().DecodeId("10042").Should().Be(42);
    }
}
