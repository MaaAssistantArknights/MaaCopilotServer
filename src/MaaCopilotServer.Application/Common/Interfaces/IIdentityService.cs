// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;

namespace MaaCopilotServer.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<CopilotUser?> GetUserAsync(Guid guid);
}
