// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;

public class CreateCopilotUserCommandValidator : AbstractValidator<CreateCopilotUserCommand>
{
    public CreateCopilotUserCommandValidator(ValidationErrorMessage errorMessage)
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
        RuleFor(x => x.Role)
            .NotEmpty().IsInEnum().NotEqual(UserRole.SuperAdmin)
            .WithMessage(errorMessage.UserRoleIsInvalid);
    }
}
