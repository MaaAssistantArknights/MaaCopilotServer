// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Extensions;

public static class DateTimeOffsetExtension
{
    public static string ToUtc8String(this DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss") + " (UTC+8)";
    }

    public static string ToIsoString(this DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.ToString("O");
    }
}
