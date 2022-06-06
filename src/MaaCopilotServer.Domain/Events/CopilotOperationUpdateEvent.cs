// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Events;

public class CopilotOperationUpdateEvent : BaseEvent
{
    public CopilotOperationUpdateEvent(CopilotOperation copilotOperation)
    {
        Operation = copilotOperation;
    }

    public CopilotOperation Operation { get; }
}
