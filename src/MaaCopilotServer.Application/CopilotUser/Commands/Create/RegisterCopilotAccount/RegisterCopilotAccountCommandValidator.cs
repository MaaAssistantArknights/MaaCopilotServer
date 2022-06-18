// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;

public class RegisterCopilotAccountCommandValidator : AbstractValidator<RegisterCopilotAccountCommand>
{
    /// <summary>
    ///     The constructor of <see cref="RegisterCopilotAccountCommandValidator" />.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public RegisterCopilotAccountCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress()
            .WithMessage(errorMessage.EmailIsInvalid);
        RuleFor(x => x.Password)
            .NotEmpty().Length(8, 32)
            .WithMessage(errorMessage.PasswordIsInvalid);
        RuleFor(x => x.UserName)
            .NotEmpty().Length(4, 24)
            .WithMessage(errorMessage.UsernameIsInvalid);
    }
}
