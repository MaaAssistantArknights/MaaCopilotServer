// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.GameData.Entity;

namespace MaaCopilotServer.GameData;

public static class Helpers
{
    public static ArkI18N BuildKeyword(ArkI18N name, ArkI18N catOne, ArkI18N catTwo, ArkI18N catThree)
    {
        var cn = new List<string>
        {
            name.ChineseSimplified, catOne.ChineseSimplified, catTwo.ChineseSimplified, catThree.ChineseSimplified
        };

        var cnT = new List<string>
        {
            name.ChineseTraditional,
            catOne.ChineseTraditional,
            catTwo.ChineseTraditional,
            catThree.ChineseTraditional
        };

        var en = new List<string> { name.English, catOne.English, catTwo.English, catThree.English };

        var jp = new List<string> { name.Japanese, catOne.Japanese, catTwo.Japanese, catThree.Japanese };

        var ko = new List<string> { name.Korean, catOne.Korean, catTwo.Korean, catThree.Korean };

        return new ArkI18N(
            cn.JoinList(),
            cnT.JoinList(),
            en.JoinList(),
            jp.JoinList(),
            ko.JoinList());
    }

    private static string JoinList(this List<string> source)
    {
        source.RemoveAll(string.IsNullOrWhiteSpace);
        return source.Count == 0 ? string.Empty : string.Join("%%", source);
    }
}
