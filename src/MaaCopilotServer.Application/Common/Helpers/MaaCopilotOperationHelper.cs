// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;

namespace MaaCopilotServer.Application.Common.Helpers;

/// <summary>
///     Helper class for JSON serialization of Maa Copilot Operation.
/// </summary>
public static class MaaCopilotOperationHelper
{
    /// <summary>
    ///     Deserializes the JSON string into <see cref="MaaCopilotOperation"/>.
    /// </summary>
    /// <param name="content">The JSON string.</param>
    /// <returns><see cref="MaaCopilotOperation"/></returns>
    public static MaaCopilotOperation? DeserializeMaaCopilotOperation(string content)
    {
        return JsonSerializer.Deserialize<MaaCopilotOperation>(content);
    }

    /// <summary>
    ///     Get Title field from <see cref="MaaCopilotOperationDoc"/> in <see cref="MaaCopilotOperation"/>.
    /// </summary>
    /// <param name="operation">The <see cref="MaaCopilotOperation"/> instance.</param>
    /// <returns>The title string, if it does not exist, it will be empty string.</returns>
    public static string GetDocTitle(this MaaCopilotOperation operation)
    {
        return operation.Doc?.Title ?? string.Empty;
    }

    /// <summary>
    ///     Get Details field from <see cref="MaaCopilotOperationDoc"/> in <see cref="MaaCopilotOperation"/>.
    /// </summary>
    /// <param name="operation">The <see cref="MaaCopilotOperation"/> instance.</param>
    /// <returns>The details string, if it does not exist, it will be empty string.</returns>
    public static string GetDocDetails(this MaaCopilotOperation operation)
    {
        return operation.Doc?.Details ?? string.Empty;
    }

    /// <summary>
    ///     Get StageName field from <see cref="MaaCopilotOperation"/>.
    /// </summary>
    /// <param name="operation">The <see cref="MaaCopilotOperation"/> instance.</param>
    /// <returns>The stageName string, if it does not exist, it will be empty string.</returns>
    public static string GetStageName(this MaaCopilotOperation operation)
    {
        return operation.StageName ?? string.Empty;
    }

    /// <summary>
    ///     Get MinimumRequired field from <see cref="MaaCopilotOperation"/>.
    /// </summary>
    /// <param name="operation">The <see cref="MaaCopilotOperation"/> instance.</param>
    /// <returns>The minimumRequired string, if it does not exist, it will be empty string.</returns>
    public static string GetMinimumRequired(this MaaCopilotOperation operation)
    {
        return operation.MinimumRequired ?? string.Empty;
    }

    /// <summary>
    ///     Serialize Groups field from <see cref="MaaCopilotOperation"/> into domain level string enumerable.
    /// </summary>
    /// <param name="operation">The <see cref="MaaCopilotOperation"/> instance.</param>
    /// <returns>String enumerable.</returns>
    public static IEnumerable<string> SerializeGroup(this MaaCopilotOperation operation)
        => (from g in operation.Groups ?? Array.Empty<MaaCopilotOperationGroup>()
            let opers = g.Operators ?? Array.Empty<MaaCopilotOperationOperator>()
            let groupOperators = opers.SerializeOperator()
            let groupOperatorString = string.Join((string?)"<>", groupOperators)
            select $"{g.Name}=>{groupOperatorString}")
            .ToList();

    /// <summary>
    ///     Serialize Operators field from <see cref="MaaCopilotOperation"/> into domain level string enumerable.
    /// </summary>
    /// <param name="operation">The <see cref="MaaCopilotOperation"/> instance.</param>
    /// <returns>String enumerable.</returns>
    public static IEnumerable<string> SerializeOperator(this MaaCopilotOperation operation)
        => (operation.Operators ?? Array.Empty<MaaCopilotOperationOperator>()).SerializeOperator();

    /// <summary>
    ///     Deserialize Groups field from domain level string enumerable into <see cref="MaaCopilotOperationGroupStore"/>.
    /// </summary>
    /// <param name="groups">Domain level string enumerable</param>
    /// <returns><see cref="MaaCopilotOperationGroupStore"/> enumerable.</returns>
    public static IEnumerable<MaaCopilotOperationGroupStore> DeserializeGroup(this string[]? groups)
        => (from g in groups ?? Array.Empty<string>()
            let groupName = g.Split("=>")[0]
            let operators = g.Split("=>")[1].Split("<>")
            select new MaaCopilotOperationGroupStore(groupName, operators.ToList()))
            .ToList();

    /// <summary>
    ///     Internal extension method to serialize <see cref="MaaCopilotOperationOperator"/>
    /// </summary>
    /// <param name="operators"></param>
    /// <returns></returns>
    private static IEnumerable<string> SerializeOperator(this IEnumerable<MaaCopilotOperationOperator> operators)
        => operators
            .Select(item => $"{item.Name}::{item.Skill ?? 1}")
            .Distinct()
            .ToList();
}
