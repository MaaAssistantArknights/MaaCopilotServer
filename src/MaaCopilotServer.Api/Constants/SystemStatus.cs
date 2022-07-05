// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;

namespace MaaCopilotServer.Api.Constants;

[ExcludeFromCodeCoverage]
public static class SystemStatus
{
    public static bool IsOk => DatabaseInitialized;

    public static bool DatabaseInitialized { get; set; } = false;
}
