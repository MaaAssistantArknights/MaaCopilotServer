// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Enums;

public enum UserRole
{
    Banned = int.MinValue,
    InActivated = -100,
    User = 0,
    Uploader = 10,
    Admin = 100,
    SuperAdmin = int.MaxValue
}
