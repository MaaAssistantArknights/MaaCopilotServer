// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MaaCopilotServer.Application.Common.Extensions;

/// <summary>
/// The extension of <see cref="FileInfo"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class FileInfoExtension
{
    /// <summary>
    ///     确认文件存在，若文件不存在，则抛出 <see cref="FileNotFoundException" /> 异常
    /// </summary>
    /// <param name="fileInfo"><see cref="FileInfo" /> 实例</param>
    /// <param name="paramName">
    ///     <see cref="CallerArgumentExpressionAttribute" />
    /// </param>
    /// <param name="memberName">
    ///     <see cref="CallerMemberNameAttribute" />
    /// </param>
    /// <returns>确保文件存在的 <see cref="FileInfo" /> 实例</returns>
    /// <exception cref="FileNotFoundException">文件不存在异常</exception>
    public static FileInfo AssertExist(this FileInfo? fileInfo,
        [CallerArgumentExpression("fileInfo")] string paramName = "UnknownParamName",
        [CallerMemberName] string memberName = "UnknownMemberName")
    {
        var fi = fileInfo.IsNotNull(paramName, memberName);
        if (fi.Exists is false)
        {
            throw new FileNotFoundException($"从 {memberName} 请求确认的 FileInfo {paramName}，文件不存在", fi.FullName);
        }

        return fi;
    }

    /// <summary>
    ///     确保文件不存在
    /// </summary>
    /// <param name="fileInfo"><see cref="FileInfo" /> 实例</param>
    /// <returns>确保文件不存在的 <see cref="FileInfo" /> 实例</returns>
    public static FileInfo EnsureDeleted(this FileInfo fileInfo)
    {
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        return fileInfo;
    }
}
