// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
/// Tests <see cref="QueryCopilotOperationsQueryValidator"/>.
/// </summary>
[TestClass]
public class QueryCopilotOperationsQueryValidatorTest
{
    /// <summary>
    /// Tests <see cref="QueryCopilotOperationsQueryValidator"/>.
    /// </summary>
    /// <param name="page">The test page.</param>
    /// <param name="limit">The test limit.</param>
    /// <param name="uploaderId">The test uploader ID.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow(null, null, null, true)]
    [DataRow(1, 2, "me", true)]
    [DataRow(1, 2, "382c74c3-721d-4f34-80e5-57657b6cbc27", true)]
    [DataRow(-1, 2, "me", false)]
    [DataRow(1, -2, "me", false)]
    [DataRow(1, 2, "invalid_guid", false)]
    public void Test(int? page, int? limit, string? uploaderId, bool expected)
    {
        ValidatorTestHelper.Test<QueryCopilotOperationsQueryValidator, QueryCopilotOperationsQuery>(new()
        {
            Page = page,
            Limit = limit,
            UploaderId = uploaderId,
        }, expected);
    }
}
