// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     用户收藏夹
/// </summary>
public class CopilotUserFavorite : EditableEntity
{
    public CopilotUserFavorite(CopilotUser user, string favoriteName)
    {
        User = user;
        FavoriteName = favoriteName;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotUserFavorite() { }
#pragma warning restore CS8618

    /// <summary>
    ///     用户
    /// </summary>
    public CopilotUser User { get; private set; }

    /// <summary>
    ///     收藏列表名
    /// </summary>
    public string FavoriteName { get; private set; }

    /// <summary>
    ///     收藏作业 GroupId
    /// </summary>
    public List<Guid> OperationGroupIds { get; private set; } = new();

    /// <summary>
    ///     收藏的作业实体 (M2M)
    /// </summary>
    public List<CopilotOperation> Operations { get; private set; } = new();

    /// <summary>
    ///     添加收藏
    /// </summary>
    /// <param name="operation"></param>
    public void AddFavoriteOperation(CopilotOperation operation)
    {
        if (OperationGroupIds.Contains(operation.GroupId))
        {
            return;
        }

        OperationGroupIds.Add(operation.GroupId);
        Operations.Add(operation);
        UpdateAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     移除收藏
    /// </summary>
    /// <param name="operation"></param>
    public void RemoveFavoriteOperation(CopilotOperation operation)
    {
        if (OperationGroupIds.Contains(operation.GroupId) is false)
        {
            return;
        }

        OperationGroupIds.Remove(operation.GroupId);
        Operations.Remove(operation);
        UpdateAt = DateTimeOffset.UtcNow;
    }
}
