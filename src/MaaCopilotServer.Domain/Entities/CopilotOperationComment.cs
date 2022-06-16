// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

/// <summary>
/// 评论
/// </summary>
public class CopilotOperationComment : BaseEntity
{
    public CopilotOperationComment(int orderId, string message, Guid replyTo, CopilotOperation operation, CopilotUser user)
    {
        OrderId = orderId;
        Message = message;
        ReplyTo = replyTo;
        Operation = operation;
        User = user;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotOperationComment() { }
#pragma warning restore CS8618

    /// <summary>
    /// 排序 ID
    /// </summary>
    public int OrderId { get; private set; }

    /// <summary>
    /// 消息体
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// 回复
    /// </summary>
    public Guid ReplyTo { get; private set; }

    /// <summary>
    /// 作业
    /// </summary>
    public CopilotOperation Operation { get; private set; }

    /// <summary>
    /// 用户
    /// </summary>
    public CopilotUser User { get; private set; }
}
