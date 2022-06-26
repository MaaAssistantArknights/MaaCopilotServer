// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Application.Test.Common.Extensions;

/// <summary>
///     Tests of <see cref="PathExtension" />.
/// </summary>
[TestClass]
public class PathExtensionTest
{
    /// <summary>
    ///     Tests <see cref="PathExtension.CombinePath(string, string)" />.
    /// </summary>
    /// <param name="path1">The first path.</param>
    /// <param name="path2">The second path.</param>
    [DataTestMethod]
    [DataRow("a/b/c", "d")]
    [DataRow("a\\b\\c", "d")]
    public void TestCombinePath(string path1, string path2)
    {
        var expected = Path.Combine(path1, path2);

        path1.CombinePath(path2).Should().Be(expected);
    }
}
