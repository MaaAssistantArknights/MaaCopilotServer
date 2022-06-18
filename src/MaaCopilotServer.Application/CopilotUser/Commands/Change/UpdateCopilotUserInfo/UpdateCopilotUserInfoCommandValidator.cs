// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;

/// <summary>
///     The validator of updating user info.
/// </summary>
public class UpdateCopilotUserInfoCommandValidator : AbstractValidator<UpdateCopilotUserInfoCommand>
{
    /// <summary>
    ///     The constructor of <see cref="UpdateCopilotUserInfoCommandValidator" />.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public UpdateCopilotUserInfoCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => x.Email is not null)
            .WithMessage(errorMessage.EmailIsInvalid);

        RuleFor(x => x.UserName)
            .NotEmpty().Length(4, 24)
            .When(x => x.UserName is not null)
            .WithMessage(errorMessage.UsernameIsInvalid);
    }
}
