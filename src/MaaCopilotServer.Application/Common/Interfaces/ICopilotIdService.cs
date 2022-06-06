// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Interfaces;

public interface ICopilotIdService
{
    string GetCopilotId(Guid id);
    Guid? GetEntityId(string copilotId);
}
