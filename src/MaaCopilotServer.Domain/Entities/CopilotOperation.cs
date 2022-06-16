// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

/// <summary>
/// Maa Copilot 作业实体
/// </summary>
public sealed class CopilotOperation : EditableEntity
{
    /// <summary>
    /// The constructor of <see cref="CopilotOperation"/>.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="details">The detail of the operation.</param>
    /// <param name="author">The author of the operation.</param>
    /// <param name="createBy">The creator of the operation.</param>
    public CopilotOperation(string content, string stageName, string minimumRequired, string title, string details, CopilotUser author, Guid createBy)
    {
        Content = content;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        Title = title;
        Details = details;
        Author = author;
        CreateBy = createBy;
        UpdateBy = createBy;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotOperation() { }
#pragma warning restore CS8618

    /// <summary>
    /// 作业 ID (数字ID)
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// 作业本体 JSON
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// 下载量
    /// </summary>
    public int Downloads { get; private set; } = 0;

    // Extract from Content

    /// <summary>
    /// 关卡名称
    /// </summary>
    public string StageName { get; private set; }
    /// <summary>
    /// MAA 所需最低版本
    /// </summary>
    public string MinimumRequired { get; private set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; private set; }
    /// <summary>
    /// 简介
    /// </summary>
    public string Details { get; private set; }
    /// <summary>
    /// 上传者
    /// </summary>
    public CopilotUser Author { get; private set; }

    /// <summary>
    /// Increases download count by 1, and updates last updated time.
    /// </summary>
    // 这个接口是可以被匿名访问的，因此不记录更新者
    public void AddDownloadCount()
    {
        Downloads++;
        UpdateAt = DateTimeOffset.UtcNow;
    }
}
