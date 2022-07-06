// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Infrastructure.Adapters;

public class SystemAdapter : ISystemAdapter
{
    public void Exit(int statusCode)
    {
        Environment.Exit(statusCode);
    }
}
