// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;

/// <summary>
/// The validator of changing user info.
/// </summary>
public class ChangeCopilotUserInfoCommandValidator : AbstractValidator<ChangeCopilotUserInfoCommand>
{
    /// <summary>
    /// The constructor of <see cref="ChangeCopilotUserInfoCommandValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public ChangeCopilotUserInfoCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.UserId)
            .NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .WithMessage(errorMessage.UserIdIsInvalid);

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress()
            .When(x => x.Email is not null)
            .WithMessage(errorMessage.EmailIsInvalid);

        RuleFor(x => x.Password)
            .NotEmpty().Length(8, 32)
            .When(x => x.Password is not null)
            .WithMessage(errorMessage.PasswordIsInvalid);

        RuleFor(x => x.UserName)
            .NotEmpty().Length(4, 24)
            .When(x => x.UserName is not null)
            .WithMessage(errorMessage.UsernameIsInvalid);

        RuleFor(x => x.Role)
            .NotEmpty().IsEnumName(typeof(UserRole)).NotEqual(UserRole.SuperAdmin.ToString())
            .When(x => x.Role is not null)
            .WithMessage(errorMessage.UserRoleIsInvalid);
    }
}
