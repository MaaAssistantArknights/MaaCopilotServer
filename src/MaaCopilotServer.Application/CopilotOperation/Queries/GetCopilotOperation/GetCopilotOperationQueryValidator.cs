// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using FluentValidation;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

public class GetCopilotOperationQueryValidator : AbstractValidator<GetCopilotOperationQuery>
{
    public GetCopilotOperationQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
    }
}
