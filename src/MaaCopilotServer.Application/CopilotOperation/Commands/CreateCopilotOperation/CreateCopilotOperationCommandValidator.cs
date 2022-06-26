// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
///     The validator of the requests of creating operation.
/// </summary>
public class CreateCopilotOperationCommandValidator : AbstractValidator<CreateCopilotOperationCommand>
{
    /// <summary>
    ///     The constructor of <see cref="CreateCopilotOperationCommandValidator" />.
    /// </summary>
    /// <param name="errorMessage">The error message when validation fails.</param>
    public CreateCopilotOperationCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Must(FluentValidationExtension.BeValidatedOperationContent)
            .WithMessage(errorMessage.CopilotOperationJsonIsInvalid);
    }
}
