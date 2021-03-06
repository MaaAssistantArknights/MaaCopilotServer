// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

public class UpdateCopilotUserPasswordCommandValidator : AbstractValidator<UpdateCopilotUserPasswordCommand>
{
    public UpdateCopilotUserPasswordCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.OriginalPassword)
            .NotEmpty().Length(8, 32)
            .WithMessage(errorMessage.PasswordIsInvalid);
        RuleFor(x => x.NewPassword)
            .NotEmpty().Length(8, 32)
            .WithMessage(errorMessage.PasswordIsInvalid);
    }
}
