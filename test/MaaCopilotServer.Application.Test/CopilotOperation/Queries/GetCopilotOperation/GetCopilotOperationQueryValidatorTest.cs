// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
/// Tests <see cref="GetCopilotOperationQueryValidator"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class GetCopilotOperationQueryValidatorTest
{
    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryValidator"/>.
    /// </summary>
    /// <param name="id">The test ID.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow("10001", true)]
    [DataRow(null, false)]
    public void Test(string? id, bool expected)
    {
        ValidatorTestHelper.Test<GetCopilotOperationQueryValidator, GetCopilotOperationQuery>(new()
        {
            Id = id,
        }, expected);
    }
}
