// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Queries.QueryCopilotUser;

/// <summary>
/// Tests of <see cref="QueryCopilotUserQueryValidator"/>.
/// </summary>
[TestClass]
public class QueryCopilotUserQueryValidatorTest
{
    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryValidator"/>.
    /// </summary>
    /// <param name="userId">The test user ID.</param>
    /// <param name="expected">The expected validation result.</param>
    [DataTestMethod]
    [DataRow(1, 10, true)]
    [DataRow(null, null, true)]
    [DataRow(0, 10, false)]
    [DataRow(1, 0, false)]
    public void Test(int? page, int? limit, bool expected)
    {
        ValidatorTestHelper.Test<QueryCopilotUserQueryValidator, QueryCopilotUserQuery>(new()
        {
            Page = page,
            Limit = limit,
        }, expected);
    }
}
