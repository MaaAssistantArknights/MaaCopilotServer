// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

/// <summary>
///     The entity to store the user's refresh token.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotUserRefreshToken : BaseEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotUserRefreshToken"/>.
    /// </summary>
    /// <param name="userId">The id of the token owner.</param>
    /// <param name="token">Refresh token string.</param>
    /// <param name="expires">Expire time.</param>
    public CopilotUserRefreshToken(Guid userId, string token, DateTimeOffset expires)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
    }
    
#pragma warning disable CS8618  
    // ReSharper disable once UnusedMember.Local
    private CopilotUserRefreshToken() { }
#pragma warning restore CS8618
    
    // WARNING:
    // YOU SHOULD NEVER EXPOSE SETTER TO PUBLIC SCOPE.
    // YOU SHOULD NEVER EXPOSE DEFAULT CONSTRUCTOR TO PUBLIC SCOPE.
    // YOU SHOULD ONLY USE A DOMAIN METHOD TO UPDATE PROPERTIES.
    // YOU SHOULD CALL DELETE METHOD BEFORE YOU ACTUALLY DELETE IT.
    
    /// <summary>
    ///     The id of the token owner.
    /// </summary>
    public Guid UserId { get; private set; }
    
    /// <summary>
    ///     Token string.
    /// </summary>
    public string Token { get; private set; }
    
    /// <summary>
    ///     Expire time.
    /// </summary>
    public DateTimeOffset Expires { get; private set; }
}
