// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Events;

public class CopilotUserDeletedEvent : BaseEvent
{
    public CopilotUserDeletedEvent(CopilotUser copilotUser)
    {
        User = copilotUser;
    }

    public CopilotUser User { get; }
}
