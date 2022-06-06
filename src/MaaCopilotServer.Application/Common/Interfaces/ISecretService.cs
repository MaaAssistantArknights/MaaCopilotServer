// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Interfaces;

public interface ISecretService
{
    (string, DateTimeOffset) GenerateJwtToken(Guid id);
    string HashPassword(string password);
    bool VerifyPassword(string hash, string password);
}
