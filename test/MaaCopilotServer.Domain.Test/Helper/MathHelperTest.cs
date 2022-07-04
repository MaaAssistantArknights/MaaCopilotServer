// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Helper;

namespace MaaCopilotServer.Domain.Test.Helper;

/// <summary>
/// Tests of <see cref="MathHelper"/>.
/// </summary>
[TestClass]
public class MathHelperTest
{
    /// <summary>
    /// Tests <see cref="MathHelper.CalculateRatio(int, int)"/>.
    /// </summary>
    [TestMethod]
    public void TestCalculateRatio()
    {
        MathHelper.CalculateRatio(0, 0).Should().Be(-1f);
        MathHelper.CalculateRatio(1, 1).Should().Be(0.5f);
        MathHelper.CalculateRatio(1, 3).Should().Be(0.25f);
    }
}
