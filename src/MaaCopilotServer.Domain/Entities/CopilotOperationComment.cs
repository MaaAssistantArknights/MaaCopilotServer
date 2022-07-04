// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Maa Copilot operation comment entity.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotOperationComment : BaseEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperationComment"/>.
    /// </summary>
    /// <param name="orderId">The sorting order id.</param>
    /// <param name="message">The comment content.</param>
    /// <param name="replyTo">The parent comment of the comment.</param>
    /// <param name="operation">The operation this comment is for.</param>
    /// <param name="user">The user who create this comment.</param>
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

    // WARNING:
    // YOU SHOULD NEVER EXPOSE SETTER TO PUBLIC SCOPE.
    // YOU SHOULD NEVER EXPOSE DEFAULT CONSTRUCTOR TO PUBLIC SCOPE.
    // YOU SHOULD ONLY USE A DOMAIN METHOD TO UPDATE PROPERTIES.
    // YOU SHOULD CALL DELETE METHOD BEFORE YOU ACTUALLY DELETE IT.

    /// <summary>
    ///     Sorting Id.
    /// </summary>
    public int OrderId { get; private set; }

    /// <summary>
    ///     The comment content.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    ///     The parent comment of the comment.
    /// </summary>
    public Guid ReplyTo { get; private set; }

    /// <summary>
    ///     The operation this comment is for.
    /// </summary>
    public CopilotOperation Operation { get; private set; }

    /// <summary>
    ///     The user who create this comment.
    /// </summary>
    public CopilotUser User { get; private set; }
}
