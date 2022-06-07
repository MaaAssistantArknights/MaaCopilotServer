// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;

public class CreateCopilotUserCommandValidator : AbstractValidator<CreateCopilotUserCommand>
{
    public CreateCopilotUserCommandValidator()
    {
        RuleFor(x => x.Email).NotNull().EmailAddress();
        RuleFor(x => x.Password).NotNull().NotEmpty().Length(8, 32);
        RuleFor(x => x.UserName).NotNull().NotEmpty().Length(4, 24);
        RuleFor(x => x.Role)
            .NotNull().NotEmpty().IsInEnum().NotEqual(UserRole.SuperAdmin);
    }
}
