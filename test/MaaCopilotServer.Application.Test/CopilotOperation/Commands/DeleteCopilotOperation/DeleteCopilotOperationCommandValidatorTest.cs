// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.DeleteCopilotOperation;

/// <summary>
/// Tests <see cref="DeleteCopilotOperationCommandValidator"/>.
/// </summary>
[TestClass]
public class DeleteCopilotOperationCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandValidator"/>.
    /// </summary>
    /// <param name="id">The test ID.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow("10000", true)]
    [DataRow(null, false)]
    public void Test(string? id, bool expected)
    {
        ValidatorTestHelper.Test<DeleteCopilotOperationCommandValidator, DeleteCopilotOperationCommand>(new()
        {
            Id = id,
        }, expected);
    }
}
