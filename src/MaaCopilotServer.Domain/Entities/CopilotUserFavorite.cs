// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
/// <summary>
///     Maa Copilot user favorite list entity.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotUserFavorite : EditableEntity
{
    /// <summary>
    ///     The constructor of <see cref="CopilotUserFavorite"/>.
    /// </summary>
    /// <param name="user">The user who own this favorite list.</param>
    /// <param name="favoriteName">The name of this list.</param>
    public CopilotUserFavorite(CopilotUser user, string favoriteName)
    {
        User = user;
        FavoriteName = favoriteName;
    }

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private CopilotUserFavorite() { }
#pragma warning restore CS8618

    // WARNING:
    // YOU SHOULD NEVER EXPOSE SETTER TO PUBLIC SCOPE.
    // YOU SHOULD NEVER EXPOSE DEFAULT CONSTRUCTOR TO PUBLIC SCOPE.
    // YOU SHOULD ONLY USE A DOMAIN METHOD TO UPDATE PROPERTIES.
    // YOU SHOULD CALL DELETE METHOD BEFORE YOU ACTUALLY DELETE IT.

    /// <summary>
    ///     The user who own this favorite list.
    /// </summary>
    public CopilotUser User { get; private set; }

    /// <summary>
    ///     The name of this list.
    /// </summary>
    public string FavoriteName { get; private set; }

    /// <summary>
    ///     The list of favorite operation ids.
    /// </summary>
    public List<Guid> OperationIds { get; private set; } = new();

    /// <summary>
    ///     The list of favorite operations. (M2M)
    /// </summary>
    public List<CopilotOperation> Operations { get; private set; } = new();

    /// <summary>
    ///     Add an operation to this favorite list.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="operator"></param>
    public void AddFavoriteOperation(CopilotOperation operation, Guid @operator)
    {
        if (OperationIds.Contains(operation.EntityId))
        {
            return;
        }

        OperationIds.Add(operation.EntityId);
        Operations.Add(operation);
        UpdateBy = @operator;
        UpdateAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Remove an operation from this favorite list.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="operator"></param>
    public void RemoveFavoriteOperation(CopilotOperation operation, Guid @operator)
    {
        if (OperationIds.Contains(operation.EntityId) is false)
        {
            return;
        }

        OperationIds.Remove(operation.EntityId);
        Operations.Remove(operation);
        UpdateBy = @operator;
        UpdateAt = DateTimeOffset.UtcNow;
    }
}
