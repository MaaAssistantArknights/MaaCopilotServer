// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;

namespace MaaCopilotServer.Api.Constants;

/// <summary>
/// The system statuses.
/// </summary>
[ExcludeFromCodeCoverage]
public static class SystemStatus
{
    /// <summary>
    /// A value that indicates whether the system has been fully initialised.
    /// </summary>
    public static bool IsOk => DatabaseInitialized && ArknightsDataInitialized;

    /// <summary>
    /// A value that indicates whether the database has been initialised. 
    /// </summary>
    public static bool DatabaseInitialized { get; set; } = false;

    /// <summary>
    /// A value that indicates whether the Arknights data has been initialised.
    /// </summary>
    public static bool ArknightsDataInitialized { get; set; } = false;
}
