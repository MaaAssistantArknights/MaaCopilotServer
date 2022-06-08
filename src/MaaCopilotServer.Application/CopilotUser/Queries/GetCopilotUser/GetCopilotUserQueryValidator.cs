// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

public class GetCopilotUserQueryValidator : AbstractValidator<GetCopilotUserQuery>
{
    public GetCopilotUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().NotEmpty().Must(FluentValidationExtension.BeValidGuid)
            .When(x => x.UserId != "me");
        RuleFor(x => x.UserId)
            .NotNull().NotEmpty().Equal("me")
            .When(x => x.UserId == "me");
    }
}
