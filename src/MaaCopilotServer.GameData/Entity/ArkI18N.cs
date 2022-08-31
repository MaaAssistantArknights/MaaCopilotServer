// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.GameData.Entity;

public class ArkI18N
{
    public ArkI18N(string chineseSimplified, string? chineseTraditional, string? english, string? japanese, string? korean)
    {
        ChineseSimplified = chineseSimplified;
        ChineseTraditional = chineseTraditional ?? string.Empty;
        English = english ?? string.Empty;
        Japanese = japanese ?? string.Empty;
        Korean = korean ?? string.Empty;
    }

    public string ChineseSimplified { get; }
    public string ChineseTraditional { get; }
    public string English { get; }
    public string Japanese { get; }
    public string Korean { get; }
}
