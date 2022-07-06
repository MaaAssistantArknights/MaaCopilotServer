// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Models;

public struct OperationValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessages { get; set; }
    public Operation.Model.Operation? Operation { get; set; }
    public Domain.Entities.ArkLevelData? ArkLevel { get; set; }
}
