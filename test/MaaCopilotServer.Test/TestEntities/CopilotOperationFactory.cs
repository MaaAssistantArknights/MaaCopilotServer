// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Test.TestEntities;

/// <summary>
/// The factory class of <see cref="CopilotOperation"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class CopilotOperationFactory : ITestEntityFactory<CopilotOperation>
{
    /// <summary>
    ///     Copilot operation id (int).
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     Copilot operation content (JSON).
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    ///     The minimum required version of MAA.
    /// </summary>
    public string MinimumRequired { get; set; } = string.Empty;

    /// <summary>
    ///     The title of the operation.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     The detail of the operation.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    ///     The uploader of the operation.
    /// </summary>
    public CopilotUser Author { get; set; } = new CopilotUserFactory().Build();

    /// <summary>
    ///     Creator GUID
    /// </summary>
    public Guid CreateBy { get; set; } = Guid.Empty;

    /// <summary>
    ///     The level this operation is made for.
    /// </summary>
    public ArkLevelData ArkLevel { get; set; } = new(new());

    /// <summary>
    ///     The operators in the operation.
    /// </summary>
    public IEnumerable<string> Operators { get; set; } = Array.Empty<string>();

    /// <summary>
    ///     The groups in the operation.
    /// </summary>
    public IEnumerable<string> Groups { get; set; } = Array.Empty<string>();

    /// <summary>
    /// The difficulty.
    /// </summary>
    public DifficultyType Difficulty { get; set; } = DifficultyType.Unknown;

    /// <inheritdoc/>
    public CopilotOperation Build()
    {
        return new CopilotOperation(Id, Content, MinimumRequired, Title, Details, Author, CreateBy, ArkLevel, Operators, Groups, Difficulty);
    }
}
