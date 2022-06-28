// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

public sealed class MapFavoriteOperation : RelationEntity
{
#pragma warning disable CS8618
    private MapFavoriteOperation() { }
#pragma warning restore CS8618

    public Guid FavoritesEntityId { get; set; }
    public CopilotUserFavorite Favorites { get; set; }

    public Guid OperationsEntityId { get; set; }
    public CopilotOperation Operations { get; set; }
}
