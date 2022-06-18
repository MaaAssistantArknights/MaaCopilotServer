// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;

public class ActivateCopilotAccountCommandValidator : AbstractValidator<ActivateCopilotAccountCommand>
{
    public ActivateCopilotAccountCommandValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage(errorMessage.TokenIsInvalid);
    }
}
