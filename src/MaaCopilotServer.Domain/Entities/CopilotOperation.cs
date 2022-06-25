// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Maa Copilot 作业实体
/// </summary>
public sealed class CopilotOperation : EditableEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperation" />.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="author">The author of the operation.</param>
    /// <param name="createBy">The creator of the operation.</param>
    public CopilotOperation(string content, string stageName, string minimumRequired, string title, string details,
        CopilotUser author, Guid createBy, List<string> operators)
    {
        GroupId = Guid.NewGuid();
        Content = content;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Author = author;
        Operators = operators;
        CreateBy = createBy;
        UpdateBy = createBy;
    }

    public CopilotOperation(Guid groupId, string content, string stageName, string minimumRequired, string title,
        string details, CopilotUser author, Guid createBy, List<string> operators)
    {
        GroupId = groupId;
        Content = content;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Author = author;
        Operators = operators;
        CreateBy = createBy;
        UpdateBy = createBy;
    }

    public CopilotOperation() { }

    /// <summary>
    ///     作业 ID (数字ID)
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     作业组 ID
    /// </summary>
    public Guid GroupId { get; set; } = Guid.Empty;

    /// <summary>
    ///     作业本体 JSON
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    ///     下载量
    /// </summary>
    public int Downloads { get; set; }

    /// <summary>
    ///     收藏量
    /// </summary>
    public int Favorites { get; set; }

    // Extract from Content

    /// <summary>
    ///     关卡名称
    /// </summary>
    public string StageName { get; set; } = string.Empty;

    /// <summary>
    ///     MAA 所需最低版本
    /// </summary>
    public string MinimumRequired { get; set; } = string.Empty;

    /// <summary>
    ///     标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     干员列表
    /// </summary>
    public List<string> Operators { get; set; } = new List<string>();

    /// <summary>
    ///     简介
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    ///     上传者
    /// </summary>
    public CopilotUser Author { get; set; } = new CopilotUser();

    /// <summary>
    ///     Increases download count by 1, and updates last updated time.
    /// </summary>
    // 这个接口是可以被匿名访问的，因此不记录更新者
    public void AddDownloadCount()
    {
        Downloads++;
        UpdateAt = DateTimeOffset.UtcNow;
    }

    // 这个接口是可以被匿名访问的，因此不记录更新者
    public void AddFavorites()
    {
        Favorites++;
        UpdateAt = DateTimeOffset.UtcNow;
    }

    // 这个接口是可以被匿名访问的，因此不记录更新者
    public void RemoveFavorites()
    {
        Favorites--;
        UpdateAt = DateTimeOffset.UtcNow;
    }
}
