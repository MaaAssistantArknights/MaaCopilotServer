// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.System.SendEmailTest;

public class SendEmailTestCommandValidator : AbstractValidator<SendEmailTestCommand>
{
    public SendEmailTestCommandValidator()
    {
        RuleFor(x => x.TargetAddress)
            .NotEmpty().EmailAddress()
            .WithMessage("INVALID EMAIL ADDRESS");
    }
}
