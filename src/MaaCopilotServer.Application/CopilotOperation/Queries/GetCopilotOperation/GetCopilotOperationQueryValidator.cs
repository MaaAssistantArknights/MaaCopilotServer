// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
/// The validator of querying operation.
/// </summary>
public class GetCopilotOperationQueryValidator : AbstractValidator<GetCopilotOperationQuery>
{
    /// <summary>
    /// The constructor of <see cref="GetCopilotOperationQueryValidator"/>.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    public GetCopilotOperationQueryValidator(ValidationErrorMessage errorMessage)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(errorMessage.CopilotOperationIdIsEmpty);
    }
}
