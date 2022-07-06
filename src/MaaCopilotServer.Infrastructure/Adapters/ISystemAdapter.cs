// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Infrastructure.Adapters;

/// <summary>
/// The adapter of system operations.
/// </summary>
public interface ISystemAdapter
{
    /// <summary>
    /// Terminates this process and returns an exit code to the operating system.
    /// </summary>
    /// <param name="statusCode">
    /// The exit code to return to the operating system. Use 0 (zero) to indicate that
    /// the process completed successfully.
    /// </param>
    void Exit(int statusCode);
}
