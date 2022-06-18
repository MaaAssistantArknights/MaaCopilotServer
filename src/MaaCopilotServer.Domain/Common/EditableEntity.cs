// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Common;

/// <summary>
///     可更改实体
/// </summary>
public abstract class EditableEntity : BaseEntity
{
    /// <summary>
    ///     更新时间
    /// </summary>
    public DateTimeOffset UpdateAt { get; protected set; } = DateTimeOffset.UtcNow;

    /// <summary>
    ///     更新者
    /// </summary>
    public Guid UpdateBy { get; protected set; }
}
