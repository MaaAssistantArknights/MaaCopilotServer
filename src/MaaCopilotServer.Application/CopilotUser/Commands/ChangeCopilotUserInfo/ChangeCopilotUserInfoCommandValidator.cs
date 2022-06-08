// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;

public class ChangeCopilotUserInfoCommandValidator : AbstractValidator<ChangeCopilotUserInfoCommand>
{
    public ChangeCopilotUserInfoCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty().Must(FluentValidationExtension.BeValidGuid);

        RuleFor(x => x.Email)
            .NotNull().EmailAddress()
            .When(x => x.Email is not null);

        RuleFor(x => x.Password)
            .NotNull().NotEmpty().Length(8, 32)
            .When(x => x.Password is not null);

        RuleFor(x => x.UserName)
            .NotNull().NotEmpty().Length(4, 24)
            .When(x => x.UserName is not null);

        RuleFor(x => x.Role)
            .NotNull().NotEmpty().IsInEnum()
            .When(x => x.Role is not null);
    }
}
