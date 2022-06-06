// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

public class QueryCopilotOperationsQueryValidator : AbstractValidator<QueryCopilotOperationsQuery>
{
    public QueryCopilotOperationsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Limit).GreaterThanOrEqualTo(1);
        RuleFor(x => x.StageName).NotNull();
        RuleFor(x => x.Content).NotNull();
    }
}
