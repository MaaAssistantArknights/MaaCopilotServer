// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Infrastructure.Services;

/// <summary>
/// The service for processing copilot ID.
/// </summary>
public class CopilotIdService : ICopilotIdService
{
    /// <summary>
    /// The minimum ID value. Other IDs should be calculated based on this value.
    /// </summary>
    // 不准改这个值!
    // DO NOT CHANGE THIS VALUE!
    // この値は変更しないでください!
    private const long MinimumId = 10000;

    /// <summary>
    /// Encodes an ID.
    /// </summary>
    /// <param name="plainId">The ID of <see cref="long"/> type.</param>
    /// <returns>The ID of <see cref="string"/> type</returns>
    public string EncodeId(long plainId)
    {
        return (plainId + MinimumId).ToString();
    }

    /// <summary>
    /// Decodes an ID.
    /// </summary>
    /// <param name="encodedId">The ID of <see cref="string"/> type.</param>
    /// <returns>The ID of <see cref="long"/> type if it is valid, otherwise <c>null</c>.</returns>
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
