// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

/// <summary>
/// The validator of updating user password.
/// </summary>
public class UpdateCopilotUserPasswordCommandValidator : AbstractValidator<UpdateCopilotUserPasswordCommand>
{
    /// <summary>
    /// The constructor of <see cref="UpdateCopilotUserPasswordCommandValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
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
