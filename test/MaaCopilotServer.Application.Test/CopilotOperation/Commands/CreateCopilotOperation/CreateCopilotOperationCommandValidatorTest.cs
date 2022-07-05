// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// Tests <see cref="CreateCopilotOperationCommandValidator"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CreateCopilotOperationCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="CreateCopilotOperationCommandValidator"/>.
    /// </summary>
    /// <param name="content">The test JSON content.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow(@"{""stage_name"": ""test_stage_name"", ""minimum_required"": ""0.0.1""}", true)]
    [DataRow(null, false)]
    [DataRow(@"{""stage_name"": null, ""minimum_required"": ""0.0.1""}", false)]
    [DataRow(@"{""stage_name"": ""test_stage_name"", ""minimum_required"": null}", false)]
    [DataRow(@"{""stage_name"": null, ""minimum_required"": null}", false)]
    public void Test(string? content, bool expected)
    {
        ValidatorTestHelper.Test<CreateCopilotOperationCommandValidator, CreateCopilotOperationCommand>(new()
        {
            Content = content,
        }, expected);
    }
}
