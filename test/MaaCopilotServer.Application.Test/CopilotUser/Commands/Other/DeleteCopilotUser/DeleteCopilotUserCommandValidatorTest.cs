// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Other.DeleteCopilotUser;

/// <summary>
/// Tests <see cref="DeleteCopilotUserCommandValidator"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class DeleteCopilotUserCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="DeleteCopilotUserCommandValidator"/>.
    /// </summary>
    /// <param name="userId">The test user ID.</param>
    /// <param name="expected">The expected validation result.</param>
    [DataTestMethod]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", true)]
    [DataRow(null, false)]
    [DataRow("invalid_guid", false)]
    public void Test(string? userId, bool expected)
    {
        ValidatorTestHelper.Test<DeleteCopilotUserCommandValidator, DeleteCopilotUserCommand>(new()
        {
            UserId = userId,
        }, expected);
    }
}
