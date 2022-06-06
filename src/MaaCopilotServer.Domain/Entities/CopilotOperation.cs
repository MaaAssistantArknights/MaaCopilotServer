// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Entities;

/// <summary>
/// Maa copilot operation definition JSON
/// </summary>
public sealed class CopilotOperation : BaseAuditableEntity
{
    public string Content { get; set; } = default!;
    public int Downloads { get; set; }

    // Extract from Content

    public string StageName { get; set; } = default!;
    public string MinimumRequired { get; set; } = default!;
    public string? Title { get; set; }
    public string? Details { get; set; }
}
