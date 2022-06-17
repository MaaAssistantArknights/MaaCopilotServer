// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Enums;

/// <summary>
/// The role of the user.
/// </summary>
public enum UserRole
{
    Banned = int.MinValue,
    InActivated = -100,
    /// <summary>
    /// The normal user.
    /// </summary>
    User = 0,

    /// <summary>
    /// The uploader.
    /// </summary>
    Uploader = 10,

    /// <summary>
    /// The administrator.
    /// </summary>
    Admin = 100,

    /// <summary>
    /// The super administrator.
    /// </summary>
    SuperAdmin = int.MaxValue
}
