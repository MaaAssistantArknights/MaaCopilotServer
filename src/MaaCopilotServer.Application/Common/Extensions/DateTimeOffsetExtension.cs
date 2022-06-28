// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Extensions;

/// <summary>
///     Extensions for <see cref="DateTimeOffset"/>.
/// </summary>
public static class DateTimeOffsetExtension
{
    /// <summary>
    ///     Get the date string in format of "yyyy-MM-dd HH:mm:ss (GMT+8)"
    /// </summary>
    /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
    /// <returns>The serialized string.</returns>
    public static string ToUtc8String(this DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss") + " (UTC+8)";
    }

    /// <summary>
    ///     Get the date string in ISO-8601 format.
    /// </summary>
    /// <param name="dateTimeOffset">The <see cref="DateTimeOffset"/> instance.</param>
    /// <returns>The serialized string.</returns>
    public static string ToIsoString(this DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.ToString("O");
    }
}
