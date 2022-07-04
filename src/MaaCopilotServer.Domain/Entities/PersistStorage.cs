// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

/// <summary>
///     Some system persistent data.
/// </summary>
public class PersistStorage : BaseEntity
{
    /// <summary>
    ///     Create new persist data.
    /// </summary>
    /// <param name="key">Data key.</param>
    /// <param name="value">Data value.</param>
    public PersistStorage(string key, string value)
    {
        Key = key;
        Value = value;

        CreateBy = Guid.Empty;
        DeleteBy = Guid.Empty;
    }

    /// <summary>
    ///     Persist data key.
    /// </summary>
    public string Key { get; private set; }
    /// <summary>
    ///     Persist data value.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    ///     Update persist data value.
    /// </summary>
    /// <param name="value">New value.</param>
    public void UpdateValue(string value)
    {
        Value = value;
    }
}
