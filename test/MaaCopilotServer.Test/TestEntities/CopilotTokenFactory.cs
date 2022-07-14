// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Test.TestEntities;

/// <summary>
/// The factory class of <see cref="CopilotToken"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotTokenFactory : ITestEntityFactory<CopilotToken>
{
    /// <summary>
    ///     The resource that this token is related to.
    /// </summary>
    public Guid ResourceId { get; set; } = Guid.Empty;

    /// <summary>
    ///     The type of the token.
    /// </summary>
    public TokenType Type { get; set; }

    /// <summary>
    ///     The token content.
    /// </summary>
    public string Token { get; set; } = HandlerTest.TestToken;

    /// <summary>
    ///     The time that this token is valid before.
    /// </summary>
    public DateTimeOffset ValidBefore { get; set; } = HandlerTest.TestTokenTimeFuture;

    /// <inheritdoc/>
    public CopilotToken Build()
    {
        return new CopilotToken(ResourceId, Type, Token, ValidBefore);
    }
}
