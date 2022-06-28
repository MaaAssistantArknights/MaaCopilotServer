// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
/// Tests for <see cref="QueryCopilotOperationsQueryValidator"/>.
/// </summary>
[TestClass]
public class QueryCopilotOperationsQueryValidatorTest
{
    /// <summary>
    /// The validation error message.
    /// </summary>
    private readonly Resources.ValidationErrorMessage _validationErrorMessage = new();

    /// <summary>
    /// Tests <see cref="QueryCopilotOperationsQueryValidator"/>.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <param name="uploaderId"></param>
    /// <param name="expected"></param>
    [DataTestMethod]
    [DataRow(null, null, null, true)]
    [DataRow(1, 2, "me", true)]
    [DataRow(1, 2, "382c74c3-721d-4f34-80e5-57657b6cbc27", true)]
    [DataRow(-1, 2, "me", false)]
    [DataRow(1, -2, "me", false)]
    [DataRow(1, 2, "invalid_guid", false)]
    public void Test(int? page, int? limit, string? uploaderId, bool expected)
    {
        var validator = new QueryCopilotOperationsQueryValidator(_validationErrorMessage);
        var data = new QueryCopilotOperationsQuery()
        {
            Page = page,
            Limit = limit,
            UploaderId = uploaderId,
        };

        var result = validator.Validate(data);

        result.IsValid.Should().Be(expected);
    }
}
