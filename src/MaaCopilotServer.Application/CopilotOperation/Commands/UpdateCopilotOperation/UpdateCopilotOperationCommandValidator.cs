// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Commands.UpdateCopilotOperation;

public class UpdateCopilotOperationCommandValidator : AbstractValidator<UpdateCopilotOperationCommand>
{
    public UpdateCopilotOperationCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(errorMessage.CopilotOperationIdIsEmpty);
        RuleFor(x => x.Content)
            .NotEmpty()
            .Must(FluentValidationExtension.BeValidatedOperationContent)
            .WithMessage(errorMessage.CopilotOperationJsonIsInvalid);
    }
}
