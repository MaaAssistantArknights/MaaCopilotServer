// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

public class QueryCopilotUserQueryValidator : AbstractValidator<QueryCopilotUserQuery>
{
    public QueryCopilotUserQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(errorMessage.PageOutOfRange);
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1).LessThanOrEqualTo(100)
            .WithMessage(errorMessage.LimitOutOfRange);
    }
}
