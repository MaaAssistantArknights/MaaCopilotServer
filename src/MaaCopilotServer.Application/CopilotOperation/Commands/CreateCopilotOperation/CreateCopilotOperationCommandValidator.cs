// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

public class CreateCopilotOperationCommandValidator : AbstractValidator<CreateCopilotOperationCommand>
{
    public CreateCopilotOperationCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Must(FluentValidationExtension.BeValidatedOperationContent)
            .WithMessage(errorMessage.CopilotOperationJsonIsInvalid);
    }
}
