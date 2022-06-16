// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Runtime.CompilerServices;

namespace MaaCopilotServer.Application.Common.Extensions;

/// <summary>
/// The extension of <see cref="DirectoryInfo"/>.
/// </summary>
public static class DirectoryInfoExtension
{
    /// <summary>
    ///     确保目录存在，若不存在，则会创建
    /// </summary>
    /// <param name="directoryInfo"><see cref="DirectoryInfo" /> 类实例</param>
    /// <param name="paramName">
    ///     <see cref="CallerArgumentExpressionAttribute" />
    /// </param>
    /// <param name="memberName">
    ///     <see cref="CallerMemberNameAttribute" />
    /// </param>
    /// <returns><see cref="DirectoryInfo" /> 类实例，并确保目录存在</returns>
    public static DirectoryInfo EnsureCreated(this DirectoryInfo? directoryInfo,
        [CallerArgumentExpression("directoryInfo")]
        string paramName = "UnknownParamName",
        [CallerMemberName] string memberName = "UnknownMemberName")
    {
        var di = directoryInfo.IsNotNull(paramName, memberName);
        if (di.Exists is false)
        {
            di.Create();
        }

        return di;
    }
}
