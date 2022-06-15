// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;

/// <summary>
/// The validator of deleting operation.
/// </summary>
public class DeleteCopilotOperationCommandValidator : AbstractValidator<DeleteCopilotOperationCommand>
{
    /// <summary>
    /// The constructor of <see cref="DeleteCopilotOperationCommandValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message when validation fails.</param>
    public DeleteCopilotOperationCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(errorMessage.CopilotOperationIdIsEmpty);
    }
}
