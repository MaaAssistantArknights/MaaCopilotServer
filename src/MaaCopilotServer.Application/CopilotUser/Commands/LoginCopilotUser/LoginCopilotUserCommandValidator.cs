// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

public class LoginCopilotUserCommandValidator : AbstractValidator<LoginCopilotUserCommand>
{
    public LoginCopilotUserCommandValidator()
    {
        RuleFor(x => x.Email).NotNull().EmailAddress();
        RuleFor(x => x.Password).NotNull().NotEmpty().Length(8, 32);
    }
}
