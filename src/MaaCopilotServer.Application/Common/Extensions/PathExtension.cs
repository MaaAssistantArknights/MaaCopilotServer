// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Extensions;

/// <summary>
///     The extension for path processing.
/// </summary>
public static class PathExtension
{
    /// <summary>
    ///     拼接路径
    /// </summary>
    /// <param name="path1">路径 1</param>
    /// <param name="path2">路径 2</param>
    /// <returns>路径 1 + 路径 2</returns>
    public static string CombinePath(this string path1, string path2)
    {
        return Path.Combine(path1, path2);
    }
}
