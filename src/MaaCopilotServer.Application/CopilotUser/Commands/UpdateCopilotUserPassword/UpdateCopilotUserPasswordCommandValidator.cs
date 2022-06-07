// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

public class UpdateCopilotUserPasswordCommandValidator : AbstractValidator<UpdateCopilotUserPasswordCommand>
{
    public UpdateCopilotUserPasswordCommandValidator()
    {
        RuleFor(x => x.OriginalPassword).NotNull().NotEmpty().Length(8, 32);
        RuleFor(x => x.NewPassword).NotNull().NotEmpty().Length(8, 32);
    }
}
