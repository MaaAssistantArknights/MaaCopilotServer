// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

namespace MaaCopilotServer.Application.Test.CopilotUser.Queries.QueryCopilotUser;

/// <summary>
/// Tests <see cref="QueryCopilotUserQueryValidator"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class QueryCopilotUserQueryValidatorTest
{
    /// <summary>
    /// Tests <see cref="QueryCopilotUserQueryValidator"/>.
    /// </summary>
    /// <param name="page">The test page number.</param>
    /// <param name="limit">The test limit number.</param>
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
