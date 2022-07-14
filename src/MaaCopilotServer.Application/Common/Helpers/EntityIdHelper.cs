// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Globalization;

namespace MaaCopilotServer.Application.Common.Helpers;

/// <summary>
/// The helper class of entity ID conversion.
/// </summary>
public static class EntityIdHelper
{
    /// <summary>
    ///     The minimum ID value. Other IDs should be calculated based on this value.
    /// </summary>
    private const long MinimumId = 10000;

    /// <summary>
    ///     Encodes an ID.
    /// </summary>
    /// <param name="plainId">The ID of <see cref="long" /> type.</param>
    /// <returns>The ID of <see cref="string" /> type</returns>
    public static string EncodeId(long plainId)
    {
        return (plainId + MinimumId).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///     Decodes an ID.
    /// </summary>
    /// <param name="encodedId">The ID of <see cref="string" /> type.</param>
    /// <returns>The ID of <see cref="long" /> type if it is valid, otherwise <c>null</c>.</returns>
    public static long? DecodeId(string encodedId)
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
