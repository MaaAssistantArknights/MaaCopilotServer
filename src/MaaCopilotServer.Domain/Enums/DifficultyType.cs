// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Domain.Enums
{
    /// <summary>
    /// Level difficulty.
    /// </summary>
    [Flags]
    public enum DifficultyType : int
    {
        /// <summary>
        /// Unknown difficulty.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Normal difficulty.
        /// </summary>
        Normal = 1 << 0,

        /// <summary>
        /// Hard (4-star) difficulty.
        /// </summary>
        Hard = 1 << 1,
    }
}
