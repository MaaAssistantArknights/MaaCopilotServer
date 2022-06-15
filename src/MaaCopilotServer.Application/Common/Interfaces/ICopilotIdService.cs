// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
/// The service for processing copilot ID.
/// </summary>
public interface ICopilotIdService
{
    /// <summary>
    /// Encodes an ID.
    /// </summary>
    /// <param name="plainId">The ID of <see cref="long"/> type.</param>
    /// <returns>The ID of <see cref="string"/> type</returns>
    string EncodeId(long plainId);

    /// <summary>
    /// Decodes an ID.
    /// </summary>
    /// <param name="encodedId">The ID of <see cref="string"/> type.</param>
    /// <returns>The ID of <see cref="long"/> type if it is valid, otherwise <c>null</c>.</returns>
    long? DecodeId(string encodedId);
}
