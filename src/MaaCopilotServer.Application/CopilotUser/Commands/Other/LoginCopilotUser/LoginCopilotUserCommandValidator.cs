// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

/// <summary>
/// The validator of user login.
/// </summary>
public class LoginCopilotUserCommandValidator : AbstractValidator<LoginCopilotUserCommand>
{
    /// <summary>
    /// The constructor of <see cref="LoginCopilotUserCommandValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
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
