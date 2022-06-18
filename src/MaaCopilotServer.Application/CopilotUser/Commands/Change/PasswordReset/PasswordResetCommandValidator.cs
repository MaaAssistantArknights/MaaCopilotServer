// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;

public class PasswordResetCommandValidator : AbstractValidator<PasswordResetCommand>
{
    public PasswordResetCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage(errorMessage.TokenIsInvalid);
        RuleFor(x => x.Password)
            .NotEmpty().Length(8, 32)
            .WithMessage(errorMessage.PasswordIsInvalid);
    }
}
