// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

public class GetCopilotOperationQueryValidator : AbstractValidator<GetCopilotOperationQuery>
{
    public GetCopilotOperationQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(errorMessage.CopilotOperationIdIsEmpty);
    }
}
