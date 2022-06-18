// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Tokens
/// </summary>
public class CopilotToken : BaseEntity
{
    public CopilotToken(Guid resourceId, TokenType type, string token, DateTimeOffset validBefore)
    {
        ResourceId = resourceId;
        Type = type;
        Token = token;
        ValidBefore = validBefore;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotToken() { }
#pragma warning restore CS8618

    public Guid ResourceId { get; private set; }
    public TokenType Type { get; private set; }
    public string Token { get; private set; }
    public DateTimeOffset ValidBefore { get; private set; }
}
