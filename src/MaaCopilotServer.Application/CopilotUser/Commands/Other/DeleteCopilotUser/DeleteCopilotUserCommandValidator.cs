// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

public class DeleteCopilotUserCommandValidator : AbstractValidator<DeleteCopilotUserCommand>
{
    public DeleteCopilotUserCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.UserId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .WithMessage(errorMessage.UserIdIsInvalid);
    }
}
