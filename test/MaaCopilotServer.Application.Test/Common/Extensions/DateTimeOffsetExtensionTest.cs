// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Application.Test.Common.Extensions;

/// <summary>
/// Tests <see cref="DateTimeOffsetExtension"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class DateTimeOffsetExtensionTest
{
    /// <summary>
    /// Tests <see cref="DateTimeOffsetExtension.ToUtc8String(DateTimeOffset)"/>.
    /// </summary>
    [TestMethod]
    public void TestToUtc8String()
    {
        new DateTimeOffset(2022, 1, 1, 0, 0, 0, new TimeSpan(0))
            .ToUtc8String()
            .Should()
            .Be("2022-01-01 08:00:00 (UTC+8)");
    }

    /// <summary>
    /// Tests <see cref="DateTimeOffsetExtension.ToIsoString(DateTimeOffset)"/>.
    /// </summary>
    [TestMethod]
    public void TestToIsoString()
    {
        new DateTimeOffset(2022, 1, 1, 0, 0, 0, new TimeSpan(0))
            .ToIsoString()
            .Should()
            .Be("2022-01-01T00:00:00.0000000+00:00");
    }
}
