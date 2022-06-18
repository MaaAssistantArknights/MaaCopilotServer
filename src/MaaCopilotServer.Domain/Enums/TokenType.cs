// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Enums;

public enum TokenType
{
    UserActivation = 0,
    UserPasswordReset = 1,
    UserEmailChange = 2,
}
