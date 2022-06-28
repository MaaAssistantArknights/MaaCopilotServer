// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Application.Test.Common.Extensions;

/// <summary>
///     Tests of <see cref="FluentValidationExtension" />.
/// </summary>
[TestClass]
public class FluentValidationExtensionTest
{
    /// <summary>
    ///     Tests <see cref="FluentValidationExtension.BeValidGuid(string?)" />.
    /// </summary>
    /// <param name="id">The ID to test.</param>
    /// <param name="isValid">The expected output.</param>
    [DataTestMethod]
    [DataRow("123e4567-e89b-12d3-a456-426614174000", true)]
    [DataRow("00000000-0000-0000-0000-000000000000", true)]
    [DataRow("not_a_valid_guid", false)]
    public void TestBeValidGuid(string id, bool isValid)
    {
        FluentValidationExtension.BeValidGuid(id).Should().Be(isValid);
    }
}
