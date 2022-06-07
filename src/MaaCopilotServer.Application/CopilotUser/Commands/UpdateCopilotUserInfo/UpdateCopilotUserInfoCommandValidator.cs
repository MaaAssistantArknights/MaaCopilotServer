// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;

public class UpdateCopilotUserInfoCommandValidator : AbstractValidator<UpdateCopilotUserInfoCommand>
{
    public UpdateCopilotUserInfoCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().EmailAddress()
            .When(x => x.Email is not null);

        RuleFor(x => x.UserName)
            .NotNull().NotEmpty().Length(4, 24)
            .When(x => x.UserName is not null);
    }
}
