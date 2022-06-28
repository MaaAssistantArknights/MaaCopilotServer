// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

public class LoginCopilotUserCommandValidator : AbstractValidator<LoginCopilotUserCommand>
{
    public LoginCopilotUserCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress()
            .WithMessage(errorMessage.LoginValidationFail);
        RuleFor(x => x.Password)
            .NotEmpty().Length(8, 32)
            .WithMessage(errorMessage.LoginValidationFail);
    }
}
