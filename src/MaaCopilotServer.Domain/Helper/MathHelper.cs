// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Helper;

public static class MathHelper
{
    /// <summary>
    ///     Calculates the Part-To-Whole ratio of two numbers.
    /// </summary>
    /// <param name="r">As the ratio part.</param>
    /// <param name="o">As the other part of the sum.</param>
    /// <returns></returns>
    public static float CalculateRatio(int r, int o)
    {
        var all = r + o;
        if (all == 0)
        {
            return -1f;
        }

        var ratio = (float)r / all;
        return MathF.Round(ratio, 4);
    }
}
