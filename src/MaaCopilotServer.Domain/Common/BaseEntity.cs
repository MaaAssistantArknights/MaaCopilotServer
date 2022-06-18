// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;

namespace MaaCopilotServer.Domain.Common;

// ReSharper disable once ConvertIfStatementToReturnStatement
/// <summary>
///     只读实体基类
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    ///     资源 ID
    /// </summary>
    [Key]
    public Guid EntityId { get; } = Guid.NewGuid();

    /// <summary>
    ///     创建者
    /// </summary>
    public Guid CreateBy { get; protected set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTimeOffset CreateAt { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    ///     是否为已删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    ///     删除者
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

    public override int GetHashCode()
    {
        return GetType().GetHashCode() * 907 + EntityId.GetHashCode();
    }

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
