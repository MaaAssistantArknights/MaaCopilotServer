// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

public class QueryCopilotUserQueryValidator : AbstractValidator<QueryCopilotUserQuery>
{
    public QueryCopilotUserQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Limit).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UserName).NotNull();
    }
}
