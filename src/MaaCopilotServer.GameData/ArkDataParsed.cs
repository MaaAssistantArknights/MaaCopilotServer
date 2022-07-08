// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.GameData.Entity;

namespace MaaCopilotServer.GameData;

public struct ArkDataParsed
{
    public List<ArkLevelEntityGlobal> ArkLevelEntities { get; init; }
    public List<ArkCharacterInfoGlobal> ArkCharacterInfos { get; init; }
}
