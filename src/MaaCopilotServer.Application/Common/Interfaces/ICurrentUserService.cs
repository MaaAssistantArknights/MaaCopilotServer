// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;

namespace MaaCopilotServer.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? GetUserIdentity();
    string GetTrackingId();
    CultureInfo GetCulture();
}
