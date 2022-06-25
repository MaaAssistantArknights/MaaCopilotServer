// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// Tests for <see cref="CreateCopilotOperationCommandValidator"/>.
/// </summary>
[TestClass]
public class CreateCopilotOperationCommandValidatorTest
{
    /// <summary>
    /// Tests when the request is valid.
    /// </summary>
    [TestMethod]
    public void Test_Valid()
    {
        var validationErrorMessage = Substitute.For<Resources.ValidationErrorMessage>();
        var validator = new CreateCopilotOperationCommandValidator(validationErrorMessage);
        var data = new CreateCopilotOperationContent()
        {
            StageName = "test_stage_name",
            MinimumRequired = "0.0.1",
        };
        var result = validator.Validate(new CreateCopilotOperationCommand()
        {
            Content = JsonSerializer.Serialize(data)
        });
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Tests when <see cref="CreateCopilotOperationCommand.Content"/> is empty.
    /// </summary>
    [TestMethod]
    public void Test_EmptyContent()
    {
        var validationErrorMessage = Substitute.For<Resources.ValidationErrorMessage>();
        var validator = new CreateCopilotOperationCommandValidator(validationErrorMessage);
        var result = validator.Validate(new CreateCopilotOperationCommand()
        {
            Content = null
        });
        result.IsValid.Should().BeFalse();
    }

    /// <summary>
    /// Tests when required fields are missing in <see cref="CreateCopilotOperationCommand.Content"/>.
    /// </summary>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version.</param>
    [DataTestMethod]
    [DataRow("test_stage_name", null)]
    [DataRow(null, "0.0.1")]
    [DataRow(null, null)]
    public void Test_MissingRequiredFields(string? stageName, string? minimumRequired)
    {
        var validationErrorMessage = Substitute.For<Resources.ValidationErrorMessage>();
        var validator = new CreateCopilotOperationCommandValidator(validationErrorMessage);
        var data = new CreateCopilotOperationContent()
        {
            StageName = stageName,
            MinimumRequired = minimumRequired,
        };
        var result = validator.Validate(new CreateCopilotOperationCommand()
        {
            Content = JsonSerializer.Serialize(data)
        });
        result.IsValid.Should().BeFalse();
    }
}
