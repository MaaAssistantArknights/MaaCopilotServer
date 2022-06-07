// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Infrastructure.Services;

public class CopilotIdService : ICopilotIdService
{
    // 不准改这个值!
    // DO NOT CHANGE THIS VALUE!
    // この値は変更しないでください!
    private const long MinimumId = 10000;

    public string EncodeId(long plainId)
    {
        return (plainId + MinimumId).ToString();
    }

    public long? DecodeId(string encodedId)
    {
        var parsable = long.TryParse(encodedId, out var value);
        if (parsable is false)
        {
            return null;
        }

        if (value < MinimumId)
        {
            return null;
        }

        return value - MinimumId;
    }
}
