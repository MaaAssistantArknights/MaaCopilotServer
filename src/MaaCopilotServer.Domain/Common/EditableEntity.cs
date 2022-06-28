// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Common;

/// <summary>
///     Editable entity base class.
/// </summary>
public abstract class EditableEntity : BaseEntity
{
    /// <summary>
    ///     Update time.
    /// </summary>
    public DateTimeOffset UpdateAt { get; protected set; } = DateTimeOffset.UtcNow;

    /// <summary>
    ///     Updater operator GUID.
    /// </summary>
    public Guid UpdateBy { get; protected set; }
}
