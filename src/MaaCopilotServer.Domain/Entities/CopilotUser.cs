// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations.Schema;

namespace MaaCopilotServer.Domain.Entities;

/// <summary>
/// Verified Maa copilot user
/// </summary>
public sealed class CopilotUser : BaseAuditableEntity
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    [NotMapped] public string Password { get; set; } = default!;
    public UserPermission Permission { get; set; } = UserPermission.Doctor;

    public List<CopilotOperation> Operations { get; set; } = new();
}
