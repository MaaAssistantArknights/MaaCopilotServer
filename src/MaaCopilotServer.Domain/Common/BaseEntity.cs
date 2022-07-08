// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MaaCopilotServer.Domain.Common;

// ReSharper disable once ConvertIfStatementToReturnStatement
/// <summary>
///     ReadOnly domain entity base class.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class BaseEntity
{
    /// <summary>
    ///     The unique entity id
    /// </summary>
    [Key]
    public Guid EntityId { get; } = Guid.NewGuid();

    /// <summary>
    ///     Creator GUID
    /// </summary>
    public Guid CreateBy { get; protected set; }

    /// <summary>
    ///     Create time
    /// </summary>
    public DateTimeOffset CreateAt { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    ///     Soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    ///     Soft delete operator GUID
    /// </summary>
    public Guid? DeleteBy { get; protected set; }

    /// <summary>
    ///     Deletes an entity by a user.
    /// </summary>
    /// <param name="operator">The user</param>
    public void Delete(Guid @operator)
    {
        DeleteBy = @operator;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity compareTo)
        {
            return false;
        }

        if (ReferenceEquals(this, compareTo))
        {
            return true;
        }

        return EntityId.Equals(compareTo.EntityId);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return GetType().GetHashCode() * 907 + EntityId.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{GetType().Name} [Id ={EntityId}]";
    }

    public static bool operator ==(BaseEntity? a, BaseEntity? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity? a, BaseEntity? b)
    {
        return !(a == b);
    }
}
