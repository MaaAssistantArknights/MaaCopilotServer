// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
/// Tests for <see cref="GetCopilotOperationQueryValidator"/>.
/// </summary>
[TestClass]
public class GetCopilotOperationQueryValidatorTest
{
    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryValidator"/>.
    /// </summary>
    [TestMethod]
    public void Test_Valid()
    {
        var validator = new GetCopilotOperationQueryValidator(new Resources.ValidationErrorMessage());
        var data = new GetCopilotOperationQuery()
        {
            Id = "10001",
        };

        var result = validator.Validate(data);

        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Tests <see cref="GetCopilotOperationQueryHandler"/> with empty ID.
    /// </summary>
    [TestMethod]
    public void Test_EmptyId()
    {
        var validator = new GetCopilotOperationQueryValidator(new Resources.ValidationErrorMessage());
        var data = new GetCopilotOperationQuery()
        {
            Id = null,
        };

        var result = validator.Validate(data);

        result.IsValid.Should().BeFalse();
    }
}
