// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;

namespace MaaCopilotServer.Application.Common.Helpers;

public static class MaaCopilotOperationHelper
{
    public static MaaCopilotOperation? DeserializeMaaCopilotOperation(string content)
    {
        return JsonSerializer.Deserialize<MaaCopilotOperation>(content);
    }

    public static string GetDocTitle(this MaaCopilotOperation operation)
    {
        return operation.Doc?.Title ?? string.Empty;
    }

    public static string GetDocDetails(this MaaCopilotOperation operation)
    {
        return operation.Doc?.Details ?? string.Empty;
    }

    public static string GetStageName(this MaaCopilotOperation operation)
    {
        return operation.StageName ?? string.Empty;
    }

    public static string GetMinimumRequired(this MaaCopilotOperation operation)
    {
        return operation.MinimumRequired ?? string.Empty;
    }

    public static IEnumerable<string> SerializeGroup(this MaaCopilotOperation operation)
        => (from g in operation.Groups ?? Array.Empty<MaaCopilotOperationGroup>()
                let opers = g.Operators ?? Array.Empty<MaaCopilotOperationOperator>()
                let groupOperators = opers.SerializeOperator()
                let groupOperatorString = string.Join((string?)"<>", groupOperators)
                select $"{g.Name}=>{groupOperatorString}")
            .ToList();

    public static IEnumerable<string> SerializeOperator(this MaaCopilotOperation operation)
        => (operation.Operators ?? Array.Empty<MaaCopilotOperationOperator>()).SerializeOperator();

    public static IEnumerable<MaaCopilotOperationGroupStore> DeserializeGroup(this string[]? groups)
        => (from g in groups ?? Array.Empty<string>()
                let groupName = g.Split("=>")[0]
                let operators = g.Split("=>")[1].Split("<>")
                select new MaaCopilotOperationGroupStore(groupName, operators.ToList()))
            .ToList();

    private static IEnumerable<string> SerializeOperator(this IEnumerable<MaaCopilotOperationOperator> operators)
        => operators
            .Select(item => $"{item.Name}::{item.Skill ?? 1}")
            .Distinct()
            .ToList();
}
