// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Operation;

public static class OperationConvertor
{
    /// <summary>
    ///     Serialize Groups field from <see cref="Model.Operation"/> into domain level string enumerable.
    /// </summary>
    /// <param name="operation">The <see cref="Model.Operation"/> instance.</param>
    /// <returns>String enumerable.</returns>
    public static IEnumerable<string> SerializeGroup(this Model.Operation operation)
        => (from g in operation.Groups ?? Array.Empty<Model.Group>()
            let opers = g.Operators ?? Array.Empty<Model.Operator>()
            let groupOperators = opers.SerializeOperator()
            let groupOperatorString = string.Join((string?)"<>", groupOperators)
            select $"{g.Name}=>{groupOperatorString}")
            .ToList();

    /// <summary>
    ///     Serialize Operators field from <see cref="Model.Operation"/> into domain level string enumerable.
    /// </summary>
    /// <param name="operation">The <see cref="Model.Operation"/> instance.</param>
    /// <returns>String enumerable.</returns>
    public static IEnumerable<string> SerializeOperator(this Model.Operation operation)
        => (operation.Operators ?? Array.Empty<Model.Operator>()).SerializeOperator();

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
    ///     Internal extension method to serialize <see cref="Model.Operator"/>
    /// </summary>
    /// <param name="operators"></param>
    /// <returns></returns>
    private static IEnumerable<string> SerializeOperator(this IEnumerable<Model.Operator> operators)
        => operators
            .Select(item => $"{item.Name}::{item.Skill ?? 1}")
            .Distinct()
            .ToList();
}
