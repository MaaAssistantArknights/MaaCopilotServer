// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
/// The validator of the requests of creating operation.
/// </summary>
public class CreateCopilotOperationCommandValidator : AbstractValidator<CreateCopilotOperationCommand>
{
    /// <summary>
    /// The constructor of <see cref="CreateCopilotOperationCommandValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message when validation fails.</param>
    public CreateCopilotOperationCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Must(BeValidatedContent)
            .WithMessage(errorMessage.CopilotOperationJsonIsInvalid);
    }

    /// <summary>
    /// Validates the content to ensure it has <c>stage_name</c> and <c>minimum_required</c> fields.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns><c>true</c> if the content is valid, <c>false</c> otherwise.</returns>
    private static bool BeValidatedContent(string? content)
    {
        try
        {
            var doc = JsonDocument.Parse(content!).RootElement;
            var stageName = doc.GetProperty("stage_name").GetString();
            var minimumRequired = doc.GetProperty("minimum_required").GetString();
            return (string.IsNullOrEmpty(stageName) is false) && (string.IsNullOrEmpty(minimumRequired) is false);
        }
        catch
        {
            return false;
        }
    }
}
