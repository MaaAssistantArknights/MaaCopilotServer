// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Maa Copilot token entity.
/// </summary>
public class CopilotToken : BaseEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotToken"/>.
    /// </summary>
    /// <param name="resourceId">The resource that this token is related to.</param>
    /// <param name="type">The type of the token.</param>
    /// <param name="token">The token content.</param>
    /// <param name="validBefore">The time that this token is valid before.</param>
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

    // WARNING:
    // YOU SHOULD NEVER EXPOSE SETTER TO PUBLIC SCOPE.
    // YOU SHOULD NEVER EXPOSE DEFAULT CONSTRUCTOR TO PUBLIC SCOPE.
    // YOU SHOULD ONLY USE A DOMAIN METHOD TO UPDATE PROPERTIES.
    // YOU SHOULD CALL DELETE METHOD BEFORE YOU ACTUALLY DELETE IT.

    /// <summary>
    ///     The resource that this token is related to.
    /// </summary>
    public Guid ResourceId { get; private set; }

    /// <summary>
    ///     The type of the token.
    /// </summary>
    public TokenType Type { get; private set; }

    /// <summary>
    ///     The token content.
    /// </summary>
    public string Token { get; private set; }

    /// <summary>
    ///     The time that this token is valid before.
    /// </summary>
    public DateTimeOffset ValidBefore { get; private set; }
}
