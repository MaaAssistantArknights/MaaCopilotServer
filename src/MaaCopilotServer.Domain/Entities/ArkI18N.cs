// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Domain.Entities;

public class ArkI18N : BaseEntity
{
    public string ChineseSimplified { get; internal set; } = string.Empty;
    public string ChineseTraditional { get; internal set;  } = string.Empty;
    public string English { get; internal set;  } = string.Empty;
    public string Japanese { get; internal set;  } = string.Empty;
    public string Korean { get; internal set;  } = string.Empty;

    internal void Update(GameData.Entity.ArkI18N i18N)
    {
        ChineseSimplified = i18N.ChineseSimplified;
        ChineseTraditional = i18N.ChineseTraditional;
        English = i18N.English;
        Japanese = i18N.Japanese;
        Korean = i18N.Korean;
    }

    public bool IsEqual(GameData.Entity.ArkI18N i18N)
    {
        return
            ChineseSimplified == i18N.ChineseSimplified &&
            ChineseTraditional == i18N.ChineseTraditional &&
            English == i18N.English &&
            Japanese == i18N.Japanese &&
            Korean == i18N.Korean;
    }
}
