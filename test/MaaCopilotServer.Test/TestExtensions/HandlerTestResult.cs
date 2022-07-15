// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;

namespace MaaCopilotServer.Test.TestExtensions;

/// <summary>
/// The handler test result.
/// </summary>
[ExcludeFromCodeCoverage]
public record HandlerTestResult
{
    /// <summary>
    /// The response.
    /// </summary>
    public MaaApiResponse Response { get; init; } = default!;

    /// <summary>
    /// The DB context.
    /// </summary>
    public IMaaCopilotDbContext DbContext { get; init; } = default!;
}
