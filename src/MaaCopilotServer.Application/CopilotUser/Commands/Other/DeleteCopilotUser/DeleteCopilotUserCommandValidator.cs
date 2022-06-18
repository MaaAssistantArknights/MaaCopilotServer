// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

/// <summary>
/// The validator of deleting user.
/// </summary>
public class DeleteCopilotUserCommandValidator : AbstractValidator<DeleteCopilotUserCommand>
{
    /// <summary>
    /// The constructor of <see cref="DeleteCopilotUserCommandValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public DeleteCopilotUserCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.UserId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .WithMessage(errorMessage.UserIdIsInvalid);
    }
}
