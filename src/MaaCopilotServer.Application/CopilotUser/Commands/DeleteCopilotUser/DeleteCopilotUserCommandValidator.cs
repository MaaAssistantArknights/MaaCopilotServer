// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

public class DeleteCopilotUserCommandValidator : AbstractValidator<DeleteCopilotUserCommand>
{
    public DeleteCopilotUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty().Must(FluentValidationExtension.BeValidGuid);
    }
}
